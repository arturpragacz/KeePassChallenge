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

using System.Linq;
using System.Windows.Forms;

using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Cryptography.KeyDerivation;
using KeePassLib.Keys;
using KeePassLib.Security;
using KeePassLib.Utility;

namespace KeePassChallenge {
	internal sealed class KPCKey : KcpCustomKey {
		private readonly KPCSettings m_settings;

		private byte[] m_challenge;
		private ProtectedBinary m_key;

		public KPCKey(string name, KPCSettings settings) : base(name) {
			m_settings = settings;
		}

		public override ProtectedBinary KeyData(PwDatabase pd) {
			var challenge = GetChallenge(pd, out var paddingPKCS);
			return KeyData(challenge, paddingPKCS);
		}

		public bool SetChallenge(byte[] challenge) {
			try {
				KeyData(challenge);
				return true;
			}
			catch (KPCOperationCancelledException) {
				return false;
			}
		}

		private ProtectedBinary KeyData(byte[] challenge, bool paddingPKCS = false) {
			if (challenge == null)
				throw new KPCException("Null challenge.");

			if (m_challenge != null && challenge.SequenceEqual(m_challenge))
				return m_key;

			byte[] response;
			using (var yubikey = new Yubikey())
				while (true) {
					using (var keyEntryForm = new KeyEntryForm(yubikey, m_settings.YubikeySlot, challenge, paddingPKCS)) {
						if (keyEntryForm.ShowDialog() == DialogResult.OK) {
							response = keyEntryForm.Response;
							break;
						}
						else { // DialogResult.No
							using (var keyFailForm = new KeyFailForm()) {
								var res = keyFailForm.ShowDialog();
								if (res == DialogResult.Retry) {
									continue;
								}
								else if (res == DialogResult.Yes) { // -> recoveryMode = true;
									response = RecoveryMode(challenge, paddingPKCS);
									break;
								}
								else { // DialogResult.Cancel -> recoveryMode = false;
									throw new KPCOperationCancelledException();
								}
							}
						}
					}
				}


			byte[] sha = CryptoUtil.HashSha256(response);
			MemUtil.ZeroByteArray(response);

			m_key = new ProtectedBinary(true, sha);
			MemUtil.ZeroByteArray(sha);

			m_challenge = challenge;

			return m_key;
		}

		private byte[] GetChallenge(PwDatabase pd, out bool paddingPKCS) {
			var challenge = pd.PublicCustomData.GetByteArray(KPCSettings.CustomDataChallengeString);
			paddingPKCS = false;

			if (challenge == null) {
				var kdf = KdfPool.Get(pd.KdfParameters.KdfUuid);
				challenge = kdf.GetSeed(pd.KdfParameters);
				paddingPKCS = true;
			}

			return challenge;
		}

		private byte[] RecoveryMode(byte[] rawChallenge, bool paddingPKCS) {
			using (var recoveryForm = new RecoveryForm()) {
				if (recoveryForm.ShowDialog() != DialogResult.OK) // DialogResult.Cancel
					throw new KPCOperationCancelledException();

				var secret = (byte[])recoveryForm.Secret.Clone();
				MemUtil.ZeroByteArray(recoveryForm.Secret);

				var response = Yubikey.Simulate(secret, rawChallenge, recoveryForm.VariableLengthChallenge, paddingPKCS);
				MemUtil.ZeroByteArray(secret);

				return response;
			}
		}
	}
}
