using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace GenericLisa
{
    public partial class Form1 : Form
    {
        private DNAWorkarea workarea;
        private DNAWorkarea area;
        private Color[,] sourceColors;
        private bool isRunning = false;
        private Thread thread;
        private double fitness = double.MaxValue;
        private int generation;
       


        public Form1()
        {
            InitializeComponent();
        }

        private void img_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (isRunning)
                    btnStart_Click(null, null);

                Bitmap sourceImg;

                try
                {
                    sourceImg = new Bitmap(openFileDialog.FileName);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                img.Image = sourceImg;
                

                sourceColors = new Color[sourceImg.Width, sourceImg.Height];

                Tools.MaxHeight = sourceImg.Height;
                Tools.MaxWidth = sourceImg.Width;

                img.Height = Tools.MaxHeight;
                img.Width = Tools.MaxWidth;

                resultImg.Height = Tools.MaxHeight;
                resultImg.Width = Tools.MaxWidth;

                for (int i = 0; i < sourceImg.Width; i++)
                {
                    for (int j = 0; j < sourceImg.Height; j++)
                    {
                        sourceColors[i, j] = sourceImg.GetPixel(i, j);
                    }
                }

                workarea = null;
                
                fitness = double.MaxValue;

                btnStart.Enabled = true;
                
            }
        }

      

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                timer.Enabled = false;
                labFitness.Text = fitness.ToString();
                btnStart.Text = "Старт";
                isRunning = false;
                KillThread();
                
            }
            else
            {
                btnStart.Text = "Стоп";
                isRunning = true;
                timer.Enabled = true;
                thread = new Thread(new ThreadStart(Start));
                thread.IsBackground = true;
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
        }

        private void Start()
        {
            if(workarea == null)
            {
                workarea = new DNAWorkarea();
                workarea.SetRandom();
            }

            while (isRunning && fitness!=0 )
            {
                DNAWorkarea newarea;
                lock (workarea)
                {
                    newarea = workarea.Clone() as DNAWorkarea;
                }

                newarea.Mutate();

                if (newarea.IsChange)
                {
                    generation++;
                    double newfitness = Fitness.GetDrawingFitness(newarea, sourceColors);

                    if(newfitness < fitness)
                    {
                       lock (workarea)
                       {
                            workarea = newarea;
                       }
                        fitness = newfitness;
                      
                    }
                }
            }
        }

        private void KillThread()
        {
            if (thread != null)
                thread.Abort();
        }
        //private void Save()
        //{
        //    Bitmap img = workarea.Draw();
        //    try
        //    {
        //        img.Save(path + (num++).ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                
        //    }
        //    catch(Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //        btnStart_Click(null, null);
        //    }
            
        //    img.Dispose();
        //}

       
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Bitmap pict = new Bitmap(320, 240);
            using(Graphics g = Graphics.FromImage(pict))
            {
                g.Clear(Color.White);
                g.DrawString("Кликните для выбора файла", new Font("Arial", 12), Brushes.Black, new PointF(35, 100));

            }
            img.Image = pict;
        }

        private void toolStripStatusLabel_Click(object sender, EventArgs e)
        {

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (workarea == null)
                return;

            int polygons = workarea.Polygons.Count;
            int points = workarea.PointCount;
            

            labFitness.Text = fitness.ToString();
            toolStripStatusGeneration.Text = generation.ToString();
            toolStripStatusPoints.Text = points.ToString();
            toolStripStatusPolygons.Text = polygons.ToString();
            

                lock (workarea)
                {
                    area = workarea.Clone() as DNAWorkarea;
                }
                resultImg.Invalidate();
                
            
        }

        private void resultImg_Paint(object sender, PaintEventArgs e)
        {
            if(workarea == null)
            {
                e.Graphics.Clear(Color.Black);
                return;
            }

            using (
                var backBuffer = new Bitmap(Tools.MaxWidth, Tools.MaxHeight,
                                            PixelFormat.Format24bppRgb))
            using (Graphics backGraphics = Graphics.FromImage(backBuffer))
            {
                backGraphics.SmoothingMode = SmoothingMode.HighQuality;
                Renderer.Render(workarea, backGraphics);

                e.Graphics.DrawImage(backBuffer, 0, 0);
            }
            //resultImg.Image = workarea.Draw();

        }

        private void resultImg_Click(object sender, EventArgs e)
        {

        }

        private void labFitness_Click(object sender, EventArgs e)
        {

        }
    }
}
