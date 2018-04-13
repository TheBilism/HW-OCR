using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading;
using OCR.MNISTLOADER;
using OCR.OBROBKAOBRAZU;
//using OCR.NEURONNETWORK;
using System.IO;

namespace testocr
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private Pen pioro;
        private Point p, old = Point.Empty;
        Bitmap DrawArea;


        //MnistLoader boob;
        public Form1()
        {
            InitializeComponent();

            Inicjacja_pol_rysowania();
        }

        private void Inicjacja_pol_rysowania()
        {
            pioro = new Pen(Color.Black, 6);
            pioro.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);

            pictureBox1.Image = DrawArea;
            pictureBox1.BackColor = Color.White;
            pictureBox2.BackColor = Color.White;
            g = Graphics.FromImage(DrawArea);
            g.SmoothingMode = SmoothingMode.HighQuality;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            old = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                p = e.Location;
                g.DrawLine(pioro, old, p);
                old = e.Location;
                
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox2.Image = pictureBox1.Image;
        }

        static string GetColumnName(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var count = 0;
            // 20800
            foreach (var image in MnistReader.ReadTestData())
            {
                //use image here     
                var arr = image.Data;

                //Debug view showing parsed image.......................
                Bitmap obraz = new Bitmap(28, 28);

                for (int y = 0; y < 28; y++)
                {
                    for (int x = 0; x < 28; x++)
                    {
                        obraz.SetPixel(x, y, Color.FromArgb(255 - arr[x, y], 255 - arr[x, y], 255 - arr[x, y])); //invert colors to have 0,0,0 be white as specified by mnist
                    }
                }
                //pictureBox3.Image = obraz;
                //pictureBox3.Refresh();

                count++;
                var litera = image.Label;
                label1.Text = GetColumnName(litera-1) + " Ilość liter :" + count;
                //label1.Text = Convert.ToString(litera) + " Ilość liter :" + count;
                label1.Update();
            }

            
            //124800
            foreach (var image in MnistReader.ReadTrainingData())
            {
                //use image here     
                var arr = image.Data;

                //Debug view showing parsed image.......................
                Bitmap obraz = new Bitmap(28, 28);

                for (int y = 0; y < 28; y++)
                {
                    for (int x = 0; x < 28; x++)
                    {
                        obraz.SetPixel(x, y, Color.FromArgb(255 - arr[x, y], 255 - arr[x, y], 255 - arr[x, y])); //invert colors to have 0,0,0 be white as specified by mnist
                    }
                }
                //pictureBox3.Image = obraz;
                //pictureBox3.Refresh();

                count++;
                var litera = image.Label;
                //Console.WriteLine(litera);
                if (Convert.ToString(litera) != label1.Text)
                {
                    label1.Text = GetColumnName(litera - 1) + " Ilość liter :" + count;
                    //label1.Text = Convert.ToString(litera) + " Ilość liter :" + count;
                    //label1.Text = ":D";
                    label1.Update();
                }
            }
            
        }

        public class DigitImage
        {
            public byte[][] pixels;
            public byte label;

            public DigitImage(byte[][] pixels,
              byte label)
            {
                this.pixels = new byte[28][];
                for (int i = 0; i < this.pixels.Length; ++i)
                    this.pixels[i] = new byte[28];

                for (int i = 0; i < 28; ++i)
                    for (int j = 0; j < 28; ++j)
                        this.pixels[i][j] = pixels[i][j];

                this.label = label;
            }

            public override string ToString()
            {
                string s = "";
                for (int i = 0; i < 28; ++i)
                {
                    for (int j = 0; j < 28; ++j)
                    {
                        if (this.pixels[i][j] == 0)
                            s += " "; // white
                        else if (this.pixels[i][j] == 255)
                            s += "O"; // black
                        else
                            s += "."; // gray
                    }
                    s += "\n";
                }
                s += "Nazwa litery " + this.label.ToString();
                return s;
            } // ToString
        }
        private void button1_Click(object sender, EventArgs e)
        {
                g.Clear(Color.White);
                pictureBox1.Image = DrawArea;
                pictureBox2.Image = pictureBox1.Image;
        }

        private void button4_Click(object sender, EventArgs e)
        {


            /*var network = new SimpleNeuralNetwork(3);

            var layerFactory = new NeuralLayerFactory();
            network.AddLayer(layerFactory.CreateNeuralLayer(3, new RectifiedActivationFuncion(),
                                                            new WeightedSumFunction()));
            network.AddLayer(layerFactory.CreateNeuralLayer(1, new SigmoidActivationFunction(0.7),
                                                            new WeightedSumFunction()));

            network.PushExpectedValues(
                new double[][] {
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 1 },
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 0 },
                new double[] { 0 },
                        });

                    network.Train(
                        new double[][] {
                new double[] { 150, 2, 0 },
                new double[] { 1002, 56, 1 },
                new double[] { 1060, 59, 1 },
                new double[] { 200, 3, 0 },
                new double[] { 300, 3, 1 },
                new double[] { 120, 1, 0 },
                new double[] { 80, 1, 0 },
                        }, 10000);

                    network.PushInputValues(new double[] { 1054, 54, 1 });
                    var outputs = network.GetOutput();
            Console.WriteLine(outputs);*/


            byte[] itb = ImageToByte(pictureBox1.Image);
            Console.WriteLine(itb);
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //recon
            //if (!Loaded)
            //    MessageBox.Show("Load the Network first!", "Warning");
            double[] wejscie_neuronu = null;
            using (Bitmap obciete = OBROBKA.Przytnij(DrawArea))
            {
                obciete.Save("p1.png");
                using (Bitmap zmniejszone = OBROBKA.Zmniejsz(obciete, 17, 20))
                {
                    zmniejszone.Save("p2.png");
                    using (Bitmap bm = new Bitmap(28, 28))
                    {
                        Graphics gr = Graphics.FromImage(bm);
                        gr.Clear(Color.White);
                        gr.DrawImage(zmniejszone, 4, 4);
                        wejscie_neuronu = OBROBKA.GetInput(bm, 0, 0, bm.Width, bm.Height, x => x.Name == "ff000000").ToArray();
                        bm.Save("p3.png");
                    }
                }
            }
        }
    }
}
