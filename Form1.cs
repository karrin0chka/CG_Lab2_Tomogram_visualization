using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;

namespace КГ_Лаб2_Визуализация_томограмм
{
    public partial class Form1 : Form
    {
        Bin bin = new Bin();
        View view = new View();
        bool loaded = false; // чтобы не запускать отрисовку, пока не загружены данные
        int currentLayer = 0; // номер слоя визуализации

        int FrameCount;
        DateTime NextFPSUpdate = DateTime.Now.AddSeconds(1);
        bool needReload = false;
        

        public Form1()
        {
            InitializeComponent();            
            bin.readBIN("‪C:\\Users\\User\\source\repos\\КГ_Лаб2_Визуализация_томограмм\\testdata.bin");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += Application_Idle;   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string str = dialog.FileName;
                bin.readBIN(str);
                view.SetupView(glControl1.Width, glControl1.Height);
                loaded = true;
                glControl1.Invalidate();
            }
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (loaded)
            {
                if (radioButton2.Checked)
                {
                    if (needReload)
                    {
                        view.generateTextureImage(currentLayer);
                        view.Load2DTexture();
                        needReload = false;
                    }
                    view.DrawTexture();
                }

                if (radioButton1.Checked)
                    view.DrawQuads(currentLayer);

                glControl1.SwapBuffers();   //загружает наш буфер в буфер экрана.
            }
        }
        private void glControl1_Paint()
        {
            if (loaded)
            {
                if (radioButton2.Checked)
                {
                    if (needReload)
                    {
                        view.generateTextureImage(currentLayer);
                        view.Load2DTexture();
                        needReload = false;
                    }
                    view.DrawTexture();
                }

                if (radioButton1.Checked)
                    view.DrawQuads(currentLayer);

                glControl1.SwapBuffers();   //загружает наш буфер в буфер экрана.
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackBar1.TickFrequency = 1;
            trackBar1.Minimum = 1;
            trackBar1.Maximum = Bin.Z - 1;
            currentLayer = trackBar1.Value;
            needReload = true;
            glControl1_Paint();            
        }

        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                displayFPS();
                glControl1.Invalidate();
            }
        }

        void displayFPS()
        {
            if (DateTime.Now >= NextFPSUpdate)
            {
                this.Text = String.Format("CT Visualizer (fps={0})", FrameCount);
                NextFPSUpdate = DateTime.Now.AddSeconds(1);
                FrameCount = 0;
            }
            FrameCount++;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar2.TickFrequency = 1;
            trackBar2.Minimum = 0;
            trackBar2.Maximum = Bin.Z - 1;
            view.min = trackBar2.Value;
            needReload = true;
            glControl1_Paint();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
        /*    trackBar3.TickFrequency = 1;
            trackBar3.Minimum = 1000;
            trackBar3.Maximum = Bin.Z - 1;
         //   view.max = trackBar3.Value;
            needReload = true;
            glControl1_Paint();*/
        }
    }
}
