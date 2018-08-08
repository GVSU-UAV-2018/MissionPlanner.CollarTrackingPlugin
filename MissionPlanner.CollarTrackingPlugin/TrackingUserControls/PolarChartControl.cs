using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.CollarTrackingPlugin.TrackingUserControls
{
    public partial class PolarChartControl : UserControl
    {
        private int size_x = 0;
        private int size_y = 0;

        private float scale_min_y = 0.0F;
        private float scale_max_y = 0.0F;



        private Graphics graphics;

        private List<KeyValuePair<int, float>> data_points 
            = new List<KeyValuePair<int, float>>();

        public PolarChartControl()
        {
            InitializeComponent();
            graphics = this.CreateGraphics();
        }

        public void AddPoint(int x, float y)
        {
            data_points.Add(new KeyValuePair<int, float>(x, y));

            if (y >= scale_min_y && y <= scale_max_y)
            {
                //within scale, just add point to graph
                DrawPoint(x, y);
            }
            else
            {
                //resize scale requires a full update
                if (y < scale_min_y)
                    scale_min_y = y;
                else
                    scale_max_y = y;

                RefreshGraph();
            }
        }

        public void Clear()
        {
            data_points.Clear();
            ResetGraph();
        }

        private void DrawPoint(int x, float y)
        {
            Pen pen = new Pen(Color.Red, 1);
            Brush brush = new SolidBrush(Color.Red);
            int point_size = 4;
            size_x = this.Size.Width - 20;
            size_y = this.Size.Height - 2;

            float angle = (float)(Math.PI * x / 180.0);
            float r = y / (scale_max_y - scale_min_y);

            //Flipped Sin and Cos for x and y because the Polar
            //Chart is clockwise in this case.
            float plot_x = 0;
            float plot_y = 0;

            if(angle % (2 * Math.PI) >= 0 && angle % (2 * Math.PI) < Math.PI / 2)
            {
                plot_x = r * (float)Math.Sin((double)angle);
                plot_y = r * (float)Math.Cos((double)angle);

                //Q1
                graphics.FillEllipse(brush, (size_x / 2.0F) + ((size_x / 2.0F) * plot_x) - (point_size / 2), 
                    (size_y / 2.0F) - ((size_y / 2.0F)*plot_y) - (point_size / 2), point_size, point_size);
            }
            else if (angle % (2 * Math.PI) >= Math.PI / 2 && angle % (2 * Math.PI) < Math.PI)
            {
                plot_x = r * (float)Math.Sin((double)angle);
                plot_y = -1 * r * (float)Math.Cos((double)angle);

                //Q4
                graphics.FillEllipse(brush, (size_x / 2.0F) + ((size_x / 2.0F) * plot_x) - (point_size / 2),
                    (size_y / 2.0F) + ((size_y / 2.0F) * plot_y) - (point_size / 2), point_size, point_size);
            }
            else if (angle % (2 * Math.PI) >= Math.PI && angle % (2 * Math.PI) < 3 * Math.PI / 2)
            {
                plot_x = -1 * r * (float)Math.Sin((double)angle);
                plot_y = -1 * r * (float)Math.Cos((double)angle);

                //Q3
                graphics.FillEllipse(brush, (size_x / 2.0F) - ((size_x / 2.0F) * plot_x) - (point_size / 2),
                    (size_y / 2.0F) + ((size_y / 2.0F) * plot_y) - (point_size / 2), point_size, point_size);
            }
            else if (angle % (2 * Math.PI) >= 3 * Math.PI / 2 && angle % (2 * Math.PI) < 2 * Math.PI)
            {
                plot_x = -1 * r * (float)Math.Sin((double)angle);
                plot_y = r * (float)Math.Cos((double)angle);

                //Q2
                graphics.FillEllipse(brush, (size_x / 2.0F) - ((size_x / 2.0F) * plot_x) - (point_size / 2),
                    (size_y / 2.0F) - ((size_y / 2.0F) * plot_y) - (point_size / 2), point_size, point_size);
            }
        }

        private void DrawEmptyChart()
        {
            Pen pen = new Pen(Color.LightBlue, 1);
            Brush brush = new SolidBrush(Color.LightBlue);
            size_x = this.Size.Width - 20;
            size_y = this.Size.Height - 2;

            //Draw horizontal cross-hair
            graphics.DrawLine(pen, 0, size_y / 2, size_x, size_y / 2);
            //Draw vertical cross-hair
            graphics.DrawLine(pen, size_x / 2, 0, size_x / 2, size_y);
            //Draw line from 30 to 210
            graphics.DrawLine(pen, (3 * size_x / 4.0F), (size_y / 2.0F) - (0.866F * (size_y / 2.0F)), size_x / 4.0F, (size_y / 2.0F) + (0.866F * (size_y / 2.0F)));
            //Draw line from 60 to 240
            graphics.DrawLine(pen, (size_x / 2.0F) - (0.866F * (size_x / 2.0F)), 3 * size_y / 4.0F, (size_x / 2.0F) + (0.866F * (size_x / 2.0F)), (size_y / 4.0F));
            //Draw line from 120 to 300
            graphics.DrawLine(pen, (3 * size_x / 4.0F), (size_y / 2.0F) + (0.866F * (size_y / 2.0F)), size_x / 4.0F, (size_y / 2.0F) - (0.866F * (size_y / 2.0F)));
            //Draw line from 150 to 330
            graphics.DrawLine(pen, (size_x / 2.0F) + (0.866F * (size_x / 2.0F)), 3 * size_y / 4.0F, (size_x / 2.0F) - (0.866F * (size_x / 2.0F)), (size_y / 4.0F));
            //Draw smallest of three circles
            graphics.DrawEllipse(pen, size_x / 3, size_y / 3, size_x / 3, size_y / 3);
            //Draw middle of three circles
            graphics.DrawEllipse(pen, size_x / 6, size_y / 6, 2 * size_x / 3, 2 * size_y / 3);
            //Draw largest of three circles
            graphics.DrawEllipse(pen, 0, 0, size_x, size_y);

            //Draw 0
            //graphics.DrawString("0", this.Font, brush, size_x / 2, 3);
            //Draw 30
            graphics.DrawString("30", this.Font, brush, 3 * size_x / 4.0F - 2, (size_y / 2.0F) - (0.866F * (size_y / 2.0F)) + 5);
            //Draw 60
            graphics.DrawString("60", this.Font, brush, (size_x / 2.0F) + (0.866F * (size_x / 2.0F)) - 10,  size_y / 4.0F + 4);
            //Draw 90
            graphics.DrawString("90", this.Font, brush, size_x - 20, size_y / 2.0F + 2);
            //Draw 120
            graphics.DrawString("120", this.Font, brush, (size_x / 2.0F) + (0.866F * (size_x / 2.0F)) - 28, 3 * size_y / 4.0F - 2);
            //Draw 150
            graphics.DrawString("150", this.Font, brush, 3 * size_x / 4.0F - 30, (size_y / 2.0F) + (0.866F * (size_y / 2.0F)) - 10);
            //Draw 180
            graphics.DrawString("180", this.Font, brush, size_x / 2 - 23, size_y - 15);
            //Draw 210
            graphics.DrawString("210", this.Font, brush, size_x / 4.0F - 15, (size_y / 2.0F) + (0.866F * (size_y / 2.0F)) - 18);
            //Draw 240
            graphics.DrawString("240", this.Font, brush, (size_x / 2.0F) - (0.866F * (size_x / 2.0F)) - 6, 3 * size_y / 4.0F - 18);
            //Draw 270
            graphics.DrawString("270", this.Font, brush, 5, size_y / 2.0F - 15);
            //Draw 300
            graphics.DrawString("300", this.Font, brush, (size_x / 2.0F) - (0.866F * (size_x / 2.0F)) + 8, size_y / 4.0F - 10);
            //Draw 330
            graphics.DrawString("330", this.Font, brush, size_x / 4.0F + 7, (size_y / 2.0F) - (0.866F * (size_y / 2.0F)) - 3);

            //Draw Max Scale
            graphics.DrawString(((double)scale_max_y).ToString(), this.Font, brush, size_x / 2.0F + 2, 2);
            //Draw 2nd Scale
            graphics.DrawString(((double)(scale_max_y - ((scale_max_y - scale_min_y) / 3))).ToString(), this.Font, brush, size_x / 2.0F + 2, size_y / 6.0F + 2);
            //Draw 3rd Scale
            graphics.DrawString(((double)(scale_min_y + ((scale_max_y - scale_min_y) / 3))).ToString(), this.Font, brush, size_x / 2.0F + 2, 2 * size_y / 6.0F + 2);
            //Draw Min Scale
            graphics.DrawString(((double)scale_min_y).ToString(), this.Font, brush, size_x / 2.0F + 2, size_y / 2.0F - 12);
        }

        private void ClearGraph()
        {
            graphics.Clear(Color.Black);
        }

        private void RefreshGraph()
        {
            ClearGraph();
            DrawEmptyChart();

            foreach (KeyValuePair<int, float> kvp in data_points)
            {
                DrawPoint(kvp.Key, kvp.Value);
            }
        }

        private void ResetGraph()
        {
            scale_min_y = 0.0F;
            scale_max_y = 0.0F;
            ClearGraph();
            DrawEmptyChart();
        }

        /*private void SetScale()
        {
            long min = 99999999999999999;
            long max = -99999999999999999;
            foreach(KeyValuePair<int, float> kvp in data_points)
            {
                if (kvp.Value > max)
                    max = (long)kvp.Value;
                else if (kvp.Value < min)
                    min = (long)kvp.Value;
            }

            scale_max_y = max + 1;
            scale_min_y = min - 1;
        }*/

        private void PolarChartControl_Resize(object sender, EventArgs e)
        {
            RefreshGraph();
        }

        private void PolarChartControl_Paint(object sender, PaintEventArgs e)
        {
            RefreshGraph();
        }
    }
}
