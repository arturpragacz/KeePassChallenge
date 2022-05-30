namespace KeePassChallenge {
	partial class RecoveryForm {
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
			this.prompt = new System.Windows.Forms.Label();
			this.secretTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.vlcCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// prompt
			// 
			this.prompt.AutoSize = true;
			this.prompt.Location = new System.Drawing.Point(142, 9);
			this.prompt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.prompt.Name = "prompt";
			this.prompt.Size = new System.Drawing.Size(116, 15);
			this.prompt.TabIndex = 0;
			this.prompt.Text = "Type your secret key.";
			// 
			// secretTextBox
			// 
			this.secretTextBox.Location = new System.Drawing.Point(34, 27);
			this.secretTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.secretTextBox.Name = "secretTextBox";
			this.secretTextBox.Size = new System.Drawing.Size(317, 23);
			this.secretTextBox.TabIndex = 1;
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(84, 80);
			this.okButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(88, 27);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(221, 79);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(88, 27);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// vlcCheckBox
			// 
			this.vlcCheckBox.AutoSize = true;
			this.vlcCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.vlcCheckBox.Location = new System.Drawing.Point(188, 55);
			this.vlcCheckBox.Margin = new System.Windows.Forms.Padding(2);
			this.vlcCheckBox.Name = "vlcCheckBox";
			this.vlcCheckBox.Size = new System.Drawing.Size(163, 19);
			this.vlcCheckBox.TabIndex = 2;
			this.vlcCheckBox.Text = "&Variable Length Challenge";
			this.vlcCheckBox.UseVisualStyleBackColor = true;
			// 
			// RecoveryForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.ClientSize = new System.Drawing.Size(384, 119);
			this.Controls.Add(this.vlcCheckBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.secretTextBox);
			this.Controls.Add(this.prompt);
			this.Name = "RecoveryForm";
			this.Text = "Key Recovery";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		
		private System.Windows.Forms.Label prompt;
		private System.Windows.Forms.TextBox secretTextBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.CheckBox vlcCheckBox;
	}
}