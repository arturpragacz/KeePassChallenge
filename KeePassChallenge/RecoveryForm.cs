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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KeePassChallenge {
	internal partial class RecoveryForm : BaseForm {
		public byte[] Secret { get; private set; }
		public bool VariableLengthChallenge { get; private set; }

		public RecoveryForm() {
			InitializeComponent();
		}

		protected override void OnFormClosing(FormClosingEventArgs eventArgs) {
			if (DialogResult == DialogResult.OK) {
				VariableLengthChallenge = vlcCheckBox.Checked;

				var secretHex = new string(secretTextBox.Text.Where(c => !Char.IsWhiteSpace(c)).ToArray());

				if (secretHex.Length == Yubikey.KeyLength * 2) {
					Secret = new byte[Yubikey.KeyLength];
					try {
						for (var i = 0; i < Secret.Length; ++i) {
							var b = secretHex.Substring(i * 2, 2);
							Secret[i] = Convert.ToByte(b, 16);
						}
					}
					catch (Exception) {
						MessageBox.Show("Error: Incorrect format of the secret.");
						eventArgs.Cancel = true;
						return;
					}
				}
				else {
					MessageBox.Show($"Error: Secret must be {Yubikey.KeyLength} bytes long.");
					eventArgs.Cancel = true;
					return;
				}
			}
		}
	}
}
