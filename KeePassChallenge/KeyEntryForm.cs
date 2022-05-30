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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using KeePassLib.Utility;

namespace KeePassChallenge {
	internal partial class KeyEntryForm : BaseForm {
		private readonly Yubikey m_yubikey;
		private readonly Yubikey.Slot m_yubikeySlot;
		private readonly byte[] m_challenge;
		private readonly bool m_paddingPKCS;
		private bool m_success;

		public byte[] Response { get; private set; }

		public KeyEntryForm(Yubikey yubikey, Yubikey.Slot yubikeySlot, byte[] challenge, bool paddingPKCS) {
			m_yubikey = yubikey;
			m_yubikeySlot = yubikeySlot;
			m_challenge = challenge;
			m_paddingPKCS = paddingPKCS;
			m_success = false;

			ControlBox = false;

			InitializeComponent();

			progressBar.Maximum = Yubikey.Timeout;
			progressBar.Value = Yubikey.Timeout;
		}

		protected override void OnLoad(EventArgs eventArgs) {
			base.OnLoad(eventArgs);

			if (!m_yubikey.OpenKey()) {
				DialogResult = DialogResult.No;
				return;
			}

			m_countdown.Start();
			m_worker.RunWorkerAsync();
		}

		private void Worker_DoWork(object sender, DoWorkEventArgs eventArgs) {
			if (m_challenge == null)
				return;

			// should return in at most 15 seconds
			Response = m_yubikey.ChallengeResponse(m_yubikeySlot, m_challenge, m_paddingPKCS);
			m_success = Response != null;
		}

		private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs eventArgs) {
			if (DialogResult == DialogResult.None) {
				if (m_success) {
					DialogResult = DialogResult.OK;
					return;
				}
				else {
					DialogResult = DialogResult.No;
				}
			}

			MemUtil.ZeroByteArray(Response);
		}

		private void Countdown_Tick(object sender, EventArgs eventArgs) {
			var countdown = (Timer)sender;
			var progress = progressBar.Value;
			progress -= countdown.Interval;

			if (progress < 0) {
				m_countdown.Stop();

				if (DialogResult != DialogResult.None)
					return;

				DialogResult = DialogResult.No;
			}
			else {
				progressBar.Value = progress;
			}
		}

		protected override void OnFormClosed(FormClosedEventArgs eventArgs) {
			base.OnFormClosed(eventArgs);
			
			try {
				m_yubikey.CloseKey();
			}
			catch (KPCException ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
