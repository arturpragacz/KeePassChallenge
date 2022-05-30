/*
	KeePassChallenge - Provides Yubikey challenge-response capability to KeePass.
	Copyright (C) 2022 Artur Pragacz

	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, version 3 of the License.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using KeePassLib.Utility;

namespace KeePassChallenge {
	internal class Yubikey : IDisposable {
		public const int KeyLength = 20;
		public const int MaxChallengeLength = 63;
		public const int ResponseLength = 20;
		public const int Timeout = 15000;

		public enum Slot {
			SLOT1 = 0,
			SLOT2 = 1
		};

		private bool m_disposed;
		private IntPtr m_key = IntPtr.Zero;


		[DllImport("libykpers-1-1.dll")]
		private static extern int yk_init();

		[DllImport("libykpers-1-1.dll")]
		private static extern int yk_release();

		[DllImport("libykpers-1-1.dll")]
		private static extern IntPtr yk_open_first_key();

		[DllImport("libykpers-1-1.dll")]
		private static extern int yk_close_key(IntPtr yk);

		[DllImport("libykpers-1-1.dll")]
		private static extern int yk_challenge_response(IntPtr yk, byte yk_cmd, int may_block, uint challenge_len, byte[] challenge, uint response_len, byte[] response);


		private static string AssemblyDirectory {
			get {
				var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetDllDirectory(string lpPathName);

		public Yubikey() {
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				string dllDir = Path.Combine(AssemblyDirectory, Environment.Is64BitProcess ? "lib\\x64" : "lib\\x86");

				if (!Directory.Exists(dllDir)) {
					throw new KPCException($"The following directory is missing:\n{dllDir}");
				}

				SetDllDirectory(dllDir);

				Marshal.PrelinkAll(typeof(Yubikey));
			}

			if (yk_init() == 0)
				throw new KPCException("Can not construct the Yubikey Interface.");
		}

		public bool OpenKey() {
			m_key = yk_open_first_key();
			if (m_key == IntPtr.Zero)
				return false;
			return true;
		}

		private readonly static ReadOnlyCollection<byte> commands = new ReadOnlyCollection<byte>(new List<byte>() {
			0x30, // SLOT_CHAL_HMAC1
			0x38  // SLOT_CHAL_HMAC2
		});

		public byte[] ChallengeResponse(Slot slot, byte[] rawChallenge, bool paddingPKCS = false) {
			if (m_key == IntPtr.Zero)
				return null;

			var challenge = rawChallenge.Take(MaxChallengeLength);
			var paddedChallenge = PadChallenge(challenge, paddingPKCS);

			uint bufLen = 64; // could be potentially smaller, but ykchalresp uses 64 bytes, so let's stick with that
			var buf = new byte[bufLen];

			try {
				var ret = yk_challenge_response(m_key, commands[(int)slot], 1, (uint)paddedChallenge.Length, paddedChallenge, bufLen, buf);
				if (ret != 0) {
					var response = new byte[ResponseLength];
					Array.Copy(buf, response, response.Length);
					return response;
				}
			}
			finally {
				MemUtil.ZeroByteArray(buf);
			}

			return null;
		}

		private static byte[] PadChallenge(IEnumerable<byte> challenge, bool paddingPKCS) {
			var padLength = 64 - challenge.Count();
			byte pad = (byte)(paddingPKCS ? padLength : ~challenge.Last());
			var paddedChallenge = challenge.Concat(Enumerable.Repeat(pad, padLength));
			return paddedChallenge.ToArray();
		}

		public void CloseKey() {
			if (m_key == IntPtr.Zero)
				return;

			if (yk_close_key(m_key) == 0) {
				throw new KPCException("Error closing Yubikey.");
			}
		}

		public static byte[] Simulate(byte[] secret, byte[] rawChallenge, bool variableLength, bool paddingPKCS = false) {
			var challenge = rawChallenge.Take(MaxChallengeLength);
			var paddedChallenge = variableLength ? challenge.ToArray() : PadChallenge(challenge, paddingPKCS);

			using (var hmac = new HMACSHA1(secret)) {
				return hmac.ComputeHash(paddedChallenge);
			}
		}

		protected virtual void Dispose(bool disposing) {
			if (m_disposed)
				return;

			if (disposing) {
				// dispose managed state (managed objects)
			}

			yk_release(); // even when it errors, we don't do anything about it

			m_disposed = true;
		}

		~Yubikey() => Dispose(disposing: false);

		public void Dispose() {
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
