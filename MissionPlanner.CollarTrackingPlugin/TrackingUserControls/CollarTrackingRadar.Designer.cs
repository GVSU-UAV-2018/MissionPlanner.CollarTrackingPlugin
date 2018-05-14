namespace MissionPlanner.CollarTrackingUI
{
    partial class CollarTrackingRadar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CollarTrackingRadarPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CollarTrackingRadarPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // CollarTrackingRadarPictureBox
            // 
            this.CollarTrackingRadarPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingRadarPictureBox.Location = new System.Drawing.Point(0, 0);
            this.CollarTrackingRadarPictureBox.Name = "CollarTrackingRadarPictureBox";
            this.CollarTrackingRadarPictureBox.Size = new System.Drawing.Size(428, 324);
            this.CollarTrackingRadarPictureBox.TabIndex = 0;
            this.CollarTrackingRadarPictureBox.TabStop = false;
            // 
            // CollarTrackingRadar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CollarTrackingRadarPictureBox);
            this.Name = "CollarTrackingRadar";
            this.Size = new System.Drawing.Size(428, 324);
            ((System.ComponentModel.ISupportInitialize)(this.CollarTrackingRadarPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox CollarTrackingRadarPictureBox;
    }
}
