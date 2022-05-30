namespace KeePassChallenge {
	partial class KeyFailForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label = new System.Windows.Forms.Label();
			this.retryButton = new System.Windows.Forms.Button();
			this.recoveryButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Location = new System.Drawing.Point(96, 9);
			this.label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(190, 15);
			this.label.TabIndex = 0;
			this.label.Text = "Yubikey challenge-response failed.";
			// 
			// retryButton
			// 
			this.retryButton.DialogResult = System.Windows.Forms.DialogResult.Retry;
			this.retryButton.Location = new System.Drawing.Point(152, 32);
			this.retryButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.retryButton.Name = "retryButton";
			this.retryButton.Size = new System.Drawing.Size(88, 29);
			this.retryButton.TabIndex = 1;
			this.retryButton.Text = "Re&try";
			this.retryButton.UseVisualStyleBackColor = true;
			// 
			// recoveryButton
			// 
			this.recoveryButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.recoveryButton.Location = new System.Drawing.Point(13, 32);
			this.recoveryButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.recoveryButton.Name = "recoveryButton";
			this.recoveryButton.Size = new System.Drawing.Size(100, 27);
			this.recoveryButton.TabIndex = 3;
			this.recoveryButton.Text = "&Recovery";
			this.recoveryButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(280, 33);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(88, 27);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// KeyFailForm
			// 
			this.AcceptButton = this.retryButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.ClientSize = new System.Drawing.Size(384, 67);
			this.Controls.Add(this.recoveryButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.retryButton);
			this.Controls.Add(this.label);
			this.Name = "KeyFailForm";
			this.Text = "Key Fail";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.Button retryButton;
		private System.Windows.Forms.Button recoveryButton;
		private System.Windows.Forms.Button cancelButton;
	}
}