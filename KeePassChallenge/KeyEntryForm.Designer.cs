namespace KeePassChallenge {
	partial class KeyEntryForm {
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
			this.components = new System.ComponentModel.Container();
			this.label = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.m_worker = new System.ComponentModel.BackgroundWorker();
			this.m_countdown = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Location = new System.Drawing.Point(95, 9);
			this.label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(141, 15);
			this.label.TabIndex = 0;
			this.label.Text = "Press the Yubikey button.";
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(13, 30);
			this.progressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(308, 29);
			this.progressBar.TabIndex = 1;
			// 
			// m_worker
			// 
			this.m_worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_DoWork);
			this.m_worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
			// 
			// m_countdown
			// 
			this.m_countdown.Interval = 500;
			this.m_countdown.Tick += new System.EventHandler(this.Countdown_Tick);
			// 
			// KeyEntryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.ClientSize = new System.Drawing.Size(334, 67);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.label);
			this.MinimizeBox = false;
			this.Name = "KeyEntryForm";
			this.Text = "Key Entry";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.ComponentModel.BackgroundWorker m_worker;
		private System.Windows.Forms.Timer m_countdown;
	}
}