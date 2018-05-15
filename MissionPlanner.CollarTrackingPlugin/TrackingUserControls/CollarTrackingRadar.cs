using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.CollarTrackingUI
{
    public partial class CollarTrackingRadar : UserControl
    {
        int WIDTH = 300, HEIGHT = 300, HAND = 150;

        int u;  //in degree
        int cx, cy;     //center of the circle
        int x, y;       //HAND coordinate

        int tx, ty, lim = 20;

        Bitmap bmp;
        Pen p;
        Graphics g;

        public CollarTrackingRadar()
        {
            InitializeComponent();
            LoadRadar();
            DrawRadar();
        }

        private void LoadRadar()
        {
            //create Bitmap
            bmp = new Bitmap(WIDTH + 1, HEIGHT + 1);

            //background color
            this.BackColor = Color.Black;

            //center
            cx = WIDTH / 2;
            cy = HEIGHT / 2;

            //initial degree of HAND
            u = 0;
        }

        private void DrawRadar()
        {
            //pen
            p = new Pen(Color.Green, 1f);

            //graphics
            g = Graphics.FromImage(bmp);

            //calculate x, y coordinate of HAND
            int tu = 0;

            if (u >= 0 && u <= 180)
            {
                //right half
                //u in degree is converted into radian.

                x = cx + (int)(HAND * Math.Sin(Math.PI * u / 180));
                y = cy - (int)(HAND * Math.Cos(Math.PI * u / 180));
            }
            else
            {
                x = cx - (int)(HAND * -Math.Sin(Math.PI * u / 180));
                y = cy - (int)(HAND * Math.Cos(Math.PI * u / 180));
            }

            if (tu >= 0 && tu <= 180)
            {
                //right half
                //tu in degree is converted into radian.

                tx = cx + (int)(HAND * Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));
            }
            else
            {
                tx = cx - (int)(HAND * -Math.Sin(Math.PI * tu / 180));
                ty = cy - (int)(HAND * Math.Cos(Math.PI * tu / 180));
            }

            //draw circle
            g.DrawEllipse(p, 0, 0, WIDTH, HEIGHT);  //bigger circle
            g.DrawEllipse(p, WIDTH / 8, HEIGHT / 8, (float)(WIDTH * 0.75), (float)(HEIGHT * 0.75));    //smaller circle

            //draw perpendicular line
            g.DrawLine(p, new Point(cx, 0), new Point(cx, HEIGHT)); // UP-DOWN
            g.DrawLine(p, new Point(0, cy), new Point(WIDTH, cy)); //LEFT-RIGHT
            g.DrawLine(p, new Point((int)(cx + 0.707*cx), (int)(0.15 * HEIGHT)), new Point((int)(cx - .707*cx), (int)(0.85 * HEIGHT))); // UP-LEFT to DOWN-RIGHT
            g.DrawLine(p, new Point((int)(cx + 0.707 * cx), (int)(0.85 * HEIGHT)), new Point((int)(cx - .707 * cx), (int)(0.15 * HEIGHT))); // UP-RIGHT to DOWN-LEFT

            //draw HAND
            g.DrawLine(new Pen(Color.Black, 1f), new Point(cx, cy), new Point(tx, ty));
            g.DrawLine(p, new Point(cx, cy), new Point(x, y));

            //load bitmap in picturebox1
            CollarTrackingRadarPictureBox.Image = bmp;

            //dispose
            p.Dispose();
            g.Dispose();

            //update
            u++;
            if (u == 360)
            {
                u = 0;
            }
        }
    }
}
