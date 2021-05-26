using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGL;
using System.Runtime.InteropServices; 

namespace myOpenGL
{
    public partial class Form1 : Form
    {
        cOGL cGL;

        public Form1()
        {

            InitializeComponent();
            cGL = new cOGL(panel1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cGL.Draw();
        }

        private void timerRepaint_Tick(object sender, EventArgs e)
        {
             cGL.Draw();
            timerRepaint.Enabled = false;
        }


        private void timerRUN_Tick(object sender, EventArgs e)
        {
            cGL.Draw(); 
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            ChessDraw.angle = e.NewValue;
            cGL.Draw();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            cGL.Draw();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            cGL.OnResize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool isMouseDown = false;
        private int x = 0;
        private int y = 0;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            x = e.X;
            y = e.Y;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isMouseDown)
            {
                int deltax = e.X-x;
                int deltay = e.Y-y;
                ChessDraw.ph += deltay / 3;
                ChessDraw.angle += deltax/3;

                cGL.Draw();

                x = e.X;
                y = e.Y;
            }
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            
        }
        public void mouse_wheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                ChessDraw.dim+=10;
                cGL.Draw();
            }
            else if (e.Delta > 0)
            {
                ChessDraw.dim-=10;
                cGL.Draw();
            }
            
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            ChessDraw.Position[0] = e.NewValue;
            cGL.Draw();
        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            ChessDraw.Position[1] = e.NewValue;
            cGL.Draw();
        }

        private void hScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            ChessDraw.Position[2] = e.NewValue;
            cGL.Draw();
        }

        private void hScrollBar5_Scroll(object sender, ScrollEventArgs e)
        {
            ChessDraw.Position[3] = e.NewValue / 10.0f;
            cGL.Draw();
        }

        private void hScrollBar7_Scroll(object sender, ScrollEventArgs e)
        {
            ChessDraw.fov = e.NewValue;
            cGL.Draw();
        }

        private void hScrollBar8_Scroll(object sender, ScrollEventArgs e)
        {
            ChessDraw.asp = e.NewValue / 10.0f;
            cGL.Draw();
        }

        private void hScrollBar9_Scroll(object sender, ScrollEventArgs e)
        {
            ChessDraw.dim = e.NewValue;
            cGL.Draw();
        }
    }
}