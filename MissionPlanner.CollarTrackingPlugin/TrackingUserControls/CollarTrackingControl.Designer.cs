namespace MissionPlanner.CollarTrackingPlugin
{
    partial class CollarTrackingControl
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, 20D);
            this.CollarTrackingControlPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CollarTrackingControlGroupBox = new System.Windows.Forms.GroupBox();
            this.CollarTrackingControlGroupBoxPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CollarFrequencyStaticLabel = new System.Windows.Forms.Label();
            this.CollarTrackingFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.MHzFrequencyLabel = new System.Windows.Forms.Label();
            this.CollarTrackingStartScanButton = new System.Windows.Forms.Button();
            this.CollarTrackingCancelScanButton = new System.Windows.Forms.Button();
            this.CollarTrackingSetFrequencyButton = new System.Windows.Forms.Button();
            this.CollarFrequencyLabel = new System.Windows.Forms.Label();
            this.CollarTrackingPolarChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CollarTrackingScanInfoLabel = new System.Windows.Forms.Label();
            this.CollarScanProgressBar = new System.Windows.Forms.ProgressBar();
            this.CollarTrackingScanBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.CollarTrackingControlPanel.SuspendLayout();
            this.CollarTrackingControlGroupBox.SuspendLayout();
            this.CollarTrackingControlGroupBoxPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollarTrackingPolarChart)).BeginInit();
            this.SuspendLayout();
            // 
            // CollarTrackingControlPanel
            // 
            this.CollarTrackingControlPanel.ColumnCount = 20;
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlPanel.Controls.Add(this.CollarTrackingControlGroupBox, 0, 0);
            this.CollarTrackingControlPanel.Controls.Add(this.CollarTrackingPolarChart, 6, 0);
            this.CollarTrackingControlPanel.Controls.Add(this.CollarTrackingScanInfoLabel, 6, 19);
            this.CollarTrackingControlPanel.Controls.Add(this.CollarScanProgressBar, 13, 19);
            this.CollarTrackingControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingControlPanel.Location = new System.Drawing.Point(0, 0);
            this.CollarTrackingControlPanel.Name = "CollarTrackingControlPanel";
            this.CollarTrackingControlPanel.RowCount = 20;
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.263157F));
            this.CollarTrackingControlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.CollarTrackingControlPanel.Size = new System.Drawing.Size(597, 333);
            this.CollarTrackingControlPanel.TabIndex = 0;
            // 
            // CollarTrackingControlGroupBox
            // 
            this.CollarTrackingControlPanel.SetColumnSpan(this.CollarTrackingControlGroupBox, 6);
            this.CollarTrackingControlGroupBox.Controls.Add(this.CollarTrackingControlGroupBoxPanel);
            this.CollarTrackingControlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingControlGroupBox.ForeColor = System.Drawing.Color.White;
            this.CollarTrackingControlGroupBox.Location = new System.Drawing.Point(3, 3);
            this.CollarTrackingControlGroupBox.Name = "CollarTrackingControlGroupBox";
            this.CollarTrackingControlPanel.SetRowSpan(this.CollarTrackingControlGroupBox, 20);
            this.CollarTrackingControlGroupBox.Size = new System.Drawing.Size(168, 327);
            this.CollarTrackingControlGroupBox.TabIndex = 0;
            this.CollarTrackingControlGroupBox.TabStop = false;
            this.CollarTrackingControlGroupBox.Text = "Collar Tracking Control";
            // 
            // CollarTrackingControlGroupBoxPanel
            // 
            this.CollarTrackingControlGroupBoxPanel.ColumnCount = 20;
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.CollarTrackingControlGroupBoxPanel.Controls.Add(this.CollarFrequencyStaticLabel, 0, 0);
            this.CollarTrackingControlGroupBoxPanel.Controls.Add(this.CollarTrackingFrequencyTextBox, 0, 2);
            this.CollarTrackingControlGroupBoxPanel.Controls.Add(this.MHzFrequencyLabel, 15, 2);
            this.CollarTrackingControlGroupBoxPanel.Controls.Add(this.CollarTrackingStartScanButton, 0, 10);
            this.CollarTrackingControlGroupBoxPanel.Controls.Add(this.CollarTrackingCancelScanButton, 0, 15);
            this.CollarTrackingControlGroupBoxPanel.Controls.Add(this.CollarTrackingSetFrequencyButton, 0, 4);
            this.CollarTrackingControlGroupBoxPanel.Controls.Add(this.CollarFrequencyLabel, 0, 1);
            this.CollarTrackingControlGroupBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingControlGroupBoxPanel.Location = new System.Drawing.Point(3, 16);
            this.CollarTrackingControlGroupBoxPanel.Name = "CollarTrackingControlGroupBoxPanel";
            this.CollarTrackingControlGroupBoxPanel.RowCount = 20;
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.CollarTrackingControlGroupBoxPanel.Size = new System.Drawing.Size(162, 308);
            this.CollarTrackingControlGroupBoxPanel.TabIndex = 0;
            // 
            // CollarFrequencyStaticLabel
            // 
            this.CollarFrequencyStaticLabel.AutoEllipsis = true;
            this.CollarFrequencyStaticLabel.AutoSize = true;
            this.CollarTrackingControlGroupBoxPanel.SetColumnSpan(this.CollarFrequencyStaticLabel, 20);
            this.CollarFrequencyStaticLabel.Location = new System.Drawing.Point(3, 0);
            this.CollarFrequencyStaticLabel.Name = "CollarFrequencyStaticLabel";
            this.CollarFrequencyStaticLabel.Size = new System.Drawing.Size(89, 13);
            this.CollarFrequencyStaticLabel.TabIndex = 0;
            this.CollarFrequencyStaticLabel.Text = "Collar Frequency:";
            this.CollarFrequencyStaticLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // CollarTrackingFrequencyTextBox
            // 
            this.CollarTrackingControlGroupBoxPanel.SetColumnSpan(this.CollarTrackingFrequencyTextBox, 15);
            this.CollarTrackingFrequencyTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingFrequencyTextBox.Location = new System.Drawing.Point(3, 43);
            this.CollarTrackingFrequencyTextBox.Name = "CollarTrackingFrequencyTextBox";
            this.CollarTrackingControlGroupBoxPanel.SetRowSpan(this.CollarTrackingFrequencyTextBox, 2);
            this.CollarTrackingFrequencyTextBox.Size = new System.Drawing.Size(114, 20);
            this.CollarTrackingFrequencyTextBox.TabIndex = 1;
            this.CollarTrackingFrequencyTextBox.TextChanged += new System.EventHandler(this.CollarTrackingFrequencyTextBox_TextChanged);
            // 
            // MHzFrequencyLabel
            // 
            this.MHzFrequencyLabel.AutoSize = true;
            this.CollarTrackingControlGroupBoxPanel.SetColumnSpan(this.MHzFrequencyLabel, 5);
            this.MHzFrequencyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MHzFrequencyLabel.Location = new System.Drawing.Point(123, 40);
            this.MHzFrequencyLabel.Name = "MHzFrequencyLabel";
            this.MHzFrequencyLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.CollarTrackingControlGroupBoxPanel.SetRowSpan(this.MHzFrequencyLabel, 2);
            this.MHzFrequencyLabel.Size = new System.Drawing.Size(36, 25);
            this.MHzFrequencyLabel.TabIndex = 2;
            this.MHzFrequencyLabel.Text = "MHz";
            // 
            // CollarTrackingStartScanButton
            // 
            this.CollarTrackingControlGroupBoxPanel.SetColumnSpan(this.CollarTrackingStartScanButton, 20);
            this.CollarTrackingStartScanButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingStartScanButton.Enabled = false;
            this.CollarTrackingStartScanButton.Location = new System.Drawing.Point(3, 158);
            this.CollarTrackingStartScanButton.Name = "CollarTrackingStartScanButton";
            this.CollarTrackingControlGroupBoxPanel.SetRowSpan(this.CollarTrackingStartScanButton, 5);
            this.CollarTrackingStartScanButton.Size = new System.Drawing.Size(156, 69);
            this.CollarTrackingStartScanButton.TabIndex = 3;
            this.CollarTrackingStartScanButton.Text = "Start Collar Frequency Scan";
            this.CollarTrackingStartScanButton.UseVisualStyleBackColor = true;
            this.CollarTrackingStartScanButton.Click += new System.EventHandler(this.CollarTrackingStartScanButton_Click);
            // 
            // CollarTrackingCancelScanButton
            // 
            this.CollarTrackingControlGroupBoxPanel.SetColumnSpan(this.CollarTrackingCancelScanButton, 20);
            this.CollarTrackingCancelScanButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingCancelScanButton.Enabled = false;
            this.CollarTrackingCancelScanButton.Location = new System.Drawing.Point(3, 233);
            this.CollarTrackingCancelScanButton.Name = "CollarTrackingCancelScanButton";
            this.CollarTrackingControlGroupBoxPanel.SetRowSpan(this.CollarTrackingCancelScanButton, 5);
            this.CollarTrackingCancelScanButton.Size = new System.Drawing.Size(156, 72);
            this.CollarTrackingCancelScanButton.TabIndex = 4;
            this.CollarTrackingCancelScanButton.Text = "Cancel Current Scan";
            this.CollarTrackingCancelScanButton.UseVisualStyleBackColor = true;
            this.CollarTrackingCancelScanButton.Click += new System.EventHandler(this.CollarTrackingCancelScanButton_Click);
            // 
            // CollarTrackingSetFrequencyButton
            // 
            this.CollarTrackingControlGroupBoxPanel.SetColumnSpan(this.CollarTrackingSetFrequencyButton, 20);
            this.CollarTrackingSetFrequencyButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingSetFrequencyButton.Location = new System.Drawing.Point(3, 68);
            this.CollarTrackingSetFrequencyButton.Name = "CollarTrackingSetFrequencyButton";
            this.CollarTrackingControlGroupBoxPanel.SetRowSpan(this.CollarTrackingSetFrequencyButton, 5);
            this.CollarTrackingSetFrequencyButton.Size = new System.Drawing.Size(156, 69);
            this.CollarTrackingSetFrequencyButton.TabIndex = 5;
            this.CollarTrackingSetFrequencyButton.Text = "Set Frequency";
            this.CollarTrackingSetFrequencyButton.UseVisualStyleBackColor = true;
            this.CollarTrackingSetFrequencyButton.Click += new System.EventHandler(this.CollarTrackingSetFrequencyButton_Click);
            // 
            // CollarFrequencyLabel
            // 
            this.CollarFrequencyLabel.AutoSize = true;
            this.CollarTrackingControlGroupBoxPanel.SetColumnSpan(this.CollarFrequencyLabel, 20);
            this.CollarFrequencyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarFrequencyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CollarFrequencyLabel.Location = new System.Drawing.Point(3, 15);
            this.CollarFrequencyLabel.Name = "CollarFrequencyLabel";
            this.CollarFrequencyLabel.Size = new System.Drawing.Size(156, 25);
            this.CollarFrequencyLabel.TabIndex = 6;
            this.CollarFrequencyLabel.Text = "None";
            this.CollarFrequencyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CollarTrackingPolarChart
            // 
            this.CollarTrackingPolarChart.BorderlineColor = System.Drawing.Color.Gray;
            this.CollarTrackingPolarChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.CollarTrackingPolarChart.ChartAreas.Add(chartArea1);
            this.CollarTrackingControlPanel.SetColumnSpan(this.CollarTrackingPolarChart, 14);
            this.CollarTrackingPolarChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingPolarChart.Location = new System.Drawing.Point(177, 3);
            this.CollarTrackingPolarChart.Name = "CollarTrackingPolarChart";
            this.CollarTrackingControlPanel.SetRowSpan(this.CollarTrackingPolarChart, 19);
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            series1.IsVisibleInLegend = false;
            series1.Name = "Series1";
            series1.Points.Add(dataPoint1);
            this.CollarTrackingPolarChart.Series.Add(series1);
            this.CollarTrackingPolarChart.Size = new System.Drawing.Size(417, 298);
            this.CollarTrackingPolarChart.TabIndex = 2;
            this.CollarTrackingPolarChart.Text = "chart1";
            // 
            // CollarTrackingScanInfoLabel
            // 
            this.CollarTrackingScanInfoLabel.AutoSize = true;
            this.CollarTrackingScanInfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CollarTrackingControlPanel.SetColumnSpan(this.CollarTrackingScanInfoLabel, 7);
            this.CollarTrackingScanInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarTrackingScanInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CollarTrackingScanInfoLabel.ForeColor = System.Drawing.Color.White;
            this.CollarTrackingScanInfoLabel.Location = new System.Drawing.Point(177, 304);
            this.CollarTrackingScanInfoLabel.Name = "CollarTrackingScanInfoLabel";
            this.CollarTrackingScanInfoLabel.Size = new System.Drawing.Size(197, 29);
            this.CollarTrackingScanInfoLabel.TabIndex = 3;
            this.CollarTrackingScanInfoLabel.Text = "Scan Status:";
            this.CollarTrackingScanInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CollarScanProgressBar
            // 
            this.CollarTrackingControlPanel.SetColumnSpan(this.CollarScanProgressBar, 7);
            this.CollarScanProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollarScanProgressBar.Location = new System.Drawing.Point(380, 307);
            this.CollarScanProgressBar.Name = "CollarScanProgressBar";
            this.CollarScanProgressBar.Size = new System.Drawing.Size(214, 23);
            this.CollarScanProgressBar.TabIndex = 4;
            // 
            // CollarTrackingScanBackgroundWorker
            // 
            this.CollarTrackingScanBackgroundWorker.WorkerReportsProgress = true;
            this.CollarTrackingScanBackgroundWorker.WorkerSupportsCancellation = true;
            this.CollarTrackingScanBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CollarTrackingScanBackgroundWorker_DoWork);
            this.CollarTrackingScanBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.CollarTrackingScanBackgroundWorker_ProgressChanged);
            this.CollarTrackingScanBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.CollarTrackingScanBackgroundWorker_RunWorkerCompleted);
            // 
            // CollarTrackingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.CollarTrackingControlPanel);
            this.Name = "CollarTrackingControl";
            this.Size = new System.Drawing.Size(597, 333);
            this.CollarTrackingControlPanel.ResumeLayout(false);
            this.CollarTrackingControlPanel.PerformLayout();
            this.CollarTrackingControlGroupBox.ResumeLayout(false);
            this.CollarTrackingControlGroupBoxPanel.ResumeLayout(false);
            this.CollarTrackingControlGroupBoxPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollarTrackingPolarChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel CollarTrackingControlPanel;
        private System.Windows.Forms.GroupBox CollarTrackingControlGroupBox;
        private System.Windows.Forms.TableLayoutPanel CollarTrackingControlGroupBoxPanel;
        private System.Windows.Forms.Label CollarFrequencyStaticLabel;
        private System.Windows.Forms.TextBox CollarTrackingFrequencyTextBox;
        private System.Windows.Forms.Label MHzFrequencyLabel;
        private System.Windows.Forms.Button CollarTrackingStartScanButton;
        private System.Windows.Forms.Button CollarTrackingCancelScanButton;
        private System.Windows.Forms.Button CollarTrackingSetFrequencyButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart CollarTrackingPolarChart;
        private System.Windows.Forms.Label CollarTrackingScanInfoLabel;
        private System.Windows.Forms.ProgressBar CollarScanProgressBar;
        private System.Windows.Forms.Label CollarFrequencyLabel;
        private System.ComponentModel.BackgroundWorker CollarTrackingScanBackgroundWorker;
    }
}
