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

using System.Diagnostics;

using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Cryptography.KeyDerivation;
using KeePassLib.Keys;

namespace KeePassChallenge {
	internal sealed class KPCKeyProvider : KeyProvider {
		private readonly KPCSettings m_settings;
		private const uint NewChallengeLength = 32;

		public override string Name => "Yubikey challenge-response";

		public override bool SecureDesktopCompatible => true;

		public KPCKeyProvider(KPCSettings settings) {
			m_settings = settings;
		}

		public override IUserKey GetCustomKey(KeyProviderQueryContext ctx) {
			if (ctx == null) { Debug.Assert(false); throw new KPCException("Null context."); }

			KPCKey key;
			if (ctx.CreatingNewKey) {
				var challenge = CryptoRandom.Instance.GetRandomBytes(NewChallengeLength);
				key = new KPCKey(Name, m_settings, challenge);
			}
			else {
				var pd = PwDatabase.LoadHeader(ctx.DatabaseIOInfo);
				var challenge = GetChallengeFromPd(pd, out var paddingPKCS);
				key = new KPCKey(Name, m_settings, challenge, paddingPKCS);
			}

			return key;
		}

		public void OnFileCreated(PwDatabase pd) {
			UpdateChallengeInPd(pd);
		}

		public void OnFileOpened(PwDatabase pd) {
			if (!pd.MasterKey.ContainsType(typeof(KPCKey)))
				return;

			//if (pd.PublicCustomData.GetTypeOf(KPCSettings.CustomDataChallengeString) == null) {
			//	var kdf = KdfPool.Get(pd.KdfParameters.KdfUuid);
			//	var challenge = kdf.GetSeed(pd.KdfParameters);
			//	pd.PublicCustomData.SetByteArray(KPCSettings.CustomDataChallengeString, challenge);
			//}

			NewChallenge(pd);
		}

		public void OnMasterKeyChanged(PwDatabase pd) {
			UpdateChallengeInPd(pd);
		}

		public void NewChallenge(PwDatabase pd) {
			if (pd.MasterKey == null)
				return;

			var key = (KPCKey)pd.MasterKey.GetUserKey(typeof(KPCKey));
			if (key == null)
				return;

			var challenge = CryptoRandom.Instance.GetRandomBytes(NewChallengeLength);
			if (!key.SetChallenge(challenge))
				return;

			pd.PublicCustomData.SetByteArray(KPCSettings.CustomDataChallengeString, challenge);
		}

		private byte[] GetChallengeFromPd(PwDatabase pd, out bool paddingPKCS) {
			var challenge = pd.PublicCustomData.GetByteArray(KPCSettings.CustomDataChallengeString);
			paddingPKCS = false;

			if (challenge == null) {
				var kdf = KdfPool.Get(pd.KdfParameters.KdfUuid);
				challenge = kdf.GetSeed(pd.KdfParameters);
				paddingPKCS = true;
			}

			return challenge;
		}

		private void UpdateChallengeInPd(PwDatabase pd) {
			var key = (KPCKey)pd.MasterKey.GetUserKey(typeof(KPCKey));
			if (key != null)
				pd.PublicCustomData.SetByteArray(KPCSettings.CustomDataChallengeString, key.Challenge);
		}
	}
}
