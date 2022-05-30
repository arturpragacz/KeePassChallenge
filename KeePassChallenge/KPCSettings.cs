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

using KeePass.App.Configuration;

namespace KeePassChallenge {
	internal class KPCSettings {
		private AceCustomConfig m_config;
		private Yubikey.Slot m_yubikeySlot;

		public const string CustomDataChallengeString = "KeePassChallenge.Challenge";
		private const string ConfigYubikeySlotString = "KeePassChallenge.YubikeySlot";

		public Yubikey.Slot YubikeySlot {
			get => m_yubikeySlot;
			set {
				m_yubikeySlot = value;
				m_config.SetLong(ConfigYubikeySlotString, (int)m_yubikeySlot + 1);
			}
		}

		public KPCSettings(AceCustomConfig config) {
			m_config = config;

			var slot = (int)m_config.GetLong(ConfigYubikeySlotString, 2) - 1;
			var yubikeySlot = Yubikey.Slot.SLOT2;
			if (Enum.IsDefined(typeof(Yubikey.Slot), slot))
				yubikeySlot = (Yubikey.Slot)slot;
			YubikeySlot = yubikeySlot;
		}
	}
}
