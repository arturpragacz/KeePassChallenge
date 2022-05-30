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

using KeePassLib.Keys;

namespace KeePassChallenge {
	internal sealed class KPCKeyProvider : KeyProvider {
		public override string Name => "Yubikey challenge-response";

		private readonly KPCSettings m_settings;

		public override bool SecureDesktopCompatible => true;

		public override IUserKey GetCustomKey(KeyProviderQueryContext ctx) {
			if (ctx == null) {
				Debug.Assert(false);
				return null;
			}

			return new KPCKey(Name, m_settings);
		}

		public KPCKeyProvider(KPCSettings settings) {
			m_settings = settings;
		}
	}
}
