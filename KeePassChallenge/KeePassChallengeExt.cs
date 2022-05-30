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
using System.Diagnostics;
using System.Windows.Forms;

using KeePass.Forms;
using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Cryptography;

namespace KeePassChallenge {
	public sealed class KeePassChallengeExt : Plugin {
		private IPluginHost m_host = null;
		private KPCKeyProvider m_prov = null;
		private KPCSettings m_settings = null;

		public override String UpdateUrl {
			get { return null; } // TODO
		}

		public override bool Initialize(IPluginHost host) {
			if (m_host != null)
				Terminate();

			if (host == null)
				return false;

			m_host = host;

			m_settings = new KPCSettings(host.CustomConfig);

			m_prov = new KPCKeyProvider(m_settings);
			m_host.KeyProviderPool.Add(m_prov);

			m_host.MainWindow.FileOpened += OnFileOpened;

			return true;
		}

		public override void Terminate() {
			if (m_host != null) {
				m_host.KeyProviderPool.Remove(m_prov);

				m_prov = null;
				m_host = null;
			}
		}

		public override ToolStripMenuItem GetMenuItem(PluginMenuType t) {
			// provide a menu item for the main location(s)
			if (t == PluginMenuType.Main) {
				var newChallenge = new ToolStripMenuItem {
					Text = "Generate a new challenge"
				};
				newChallenge.Click += (s, e) => NewChallenge(m_host.Database);

				var yubikeySlot1 = new ToolStripMenuItem {
					Text = "Slot 1",
					CheckOnClick = true,
					Checked = m_settings.YubikeySlot == Yubikey.Slot.SLOT1
				};
				var yubikeySlot2 = new ToolStripMenuItem {
					Text = "Slot 2",
					CheckOnClick = true,
					Checked = m_settings.YubikeySlot == Yubikey.Slot.SLOT2
				};
				yubikeySlot1.Click += (s, e) => { yubikeySlot2.Checked = false; m_settings.YubikeySlot = Yubikey.Slot.SLOT1; };
				yubikeySlot2.Click += (s, e) => { yubikeySlot1.Checked = false; m_settings.YubikeySlot = Yubikey.Slot.SLOT2; };

				var yubikeySlotSelect = new ToolStripMenuItem {
					Text = "Select Yubikey Slot"
				};
				yubikeySlotSelect.DropDownItems.AddRange(new ToolStripItem[] { yubikeySlot1, yubikeySlot2 });

				var rootMenuItem = new ToolStripMenuItem() {
					Text = "KeePassChallenge"
				};
				rootMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newChallenge, yubikeySlotSelect });

				return rootMenuItem;
			}

			return null; // no menu items in other locations
		}

		private void NewChallenge(PwDatabase pd) {
			if (pd.MasterKey == null)
				return;

			var key = (KPCKey)pd.MasterKey.GetUserKey(typeof(KPCKey));
			if (key != null) {
				var challenge = CryptoRandom.Instance.GetRandomBytes(32);
				if (key.SetChallenge(challenge))
					pd.PublicCustomData.SetByteArray(KPCSettings.CustomDataChallengeString, challenge);
			}
		}

		private void OnFileOpened(object sender, FileOpenedEventArgs e) {
			if (e == null) { Debug.Assert(false); return; }
			var pd = e.Database;

			if (!pd.MasterKey.ContainsType(typeof(KPCKey)))
				return;

			//if (pd.PublicCustomData.GetTypeOf(KPCSettings.CustomDataChallengeString) == null) {
			//	var kdf = KdfPool.Get(pd.KdfParameters.KdfUuid);
			//	var challenge = kdf.GetSeed(pd.KdfParameters);
			//	pd.PublicCustomData.SetByteArray(KPCSettings.CustomDataChallengeString, challenge);
			//}

			NewChallenge(pd);
		}
	}
}
