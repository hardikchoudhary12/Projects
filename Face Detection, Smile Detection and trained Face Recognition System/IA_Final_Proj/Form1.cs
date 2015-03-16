//om ganganpatye namha
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;


using System.IO;
using System.Diagnostics;



namespace IA_Final_Proj
{
    public partial class Form1 : Form
    {

        private HaarCascade haar;
        private HaarCascade haar2;
        Image<Bgr, byte> img1;
        Image<Bgr, byte> img2;
        int index;
        int ch = 0;
        Bitmap[] arr;
        int[,] histo = new int[8, 128];
        int[,] histo2 = new int[8, 128];

        int[,] key;
        int globe;
        int ch1 = 0;

       // Bitmap[] arr;
        int[,] histogram1 = new int[8, 8];
        int[,] histogram12 = new int[8, 8];
        int bi;
        int[] mag = new int[100];
        double[] tan = new double[100];

        Image<Bgr, Byte> mycurrentFrame;

        HaarCascade myface;
        Capture mygrabber;

        MCvFont myfont = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        HaarCascade myeye;


        Image<Gray, byte> mygray = null;
        Image<Gray, byte> myresult, Total_TrainedFace = null;

        List<string> mylabels = new List<string>();
        List<Image<Gray, byte>> mytrainingImages = new List<Image<Gray, byte>>();


        int CountTrain, NumberLabels, t_var;

        string his_name, all_names = null;
        List<string> all_Persons = new List<string>();




        public Form1()
        {
            InitializeComponent();
            myface = new HaarCascade("haarcascade_frontalface_default.xml");
            Debug.WriteLine("Send to debug output.");
            Console.Write("One ");

            try
            {

                string all_Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces_folder/TrainedLabels_file.txt");
                string[] all_Labels = all_Labelsinfo.Split('%');
                NumberLabels = Convert.ToInt16(all_Labels[0]);
                CountTrain = NumberLabels;
                string myLoadFaces;

                for (int i = 1; i < NumberLabels + 1; i++)
                {
                    myLoadFaces = "face" + i + ".bmp";
                    mytrainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces_folder/" + myLoadFaces));
                    mylabels.Add(all_Labels[i]);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("training folder is still empty", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //face1();//1
         //   click();
           // facee();
          //  smile();
            //
          // SIFT_help();
        }

        public void face1()
        {

            Image<Bgr, byte> img1 = new Image<Bgr, byte>("D:\\hum.tif");

            Image<Bgr, byte> img2 = new Image<Bgr, byte>(img1.Size);

            Image<Bgr, byte> img_rslt = new Image<Bgr, byte>(img1.Size);

            for (int i = 0; i < img_rslt.Height; i++)
            {
                for (int j = 0; j < img_rslt.Width; j++)
                {
                    img_rslt.Data[i, j, 0] = img1.Data[i, j, 0];
                    img_rslt.Data[i, j, 1] = img1.Data[i, j, 1];
                    img_rslt.Data[i, j, 2] = img1.Data[i, j, 2];
                }
            }



            for (int i = 0; i < img_rslt.Height; i++)
            {
                for (int j = 0; j < img_rslt.Width; j++)
                {

                    // normalized red  r = R / (R + G + B)
                    //normalized green g = G / (R + G + B)
                    //normalized blue b = B / (R + G + B)

                    float b, r, g;
                    int R, G, B;
                    R = img1.Data[i, j, 2];
                    G = img1.Data[i, j, 1];
                    B = img1.Data[i, j, 0];
                    if (R + G + B == 0)
                    {
                        b = 0;
                        g = 0;
                        r = 0;
                    }
                    else
                    {
                        b = (float)(B * 100 / (R + G + B));
                        g = (float)(G * 100 / (R + G + B));
                        r = (float)(R * 100 / (R + G + B));
                    }
                  
                    if (r >= 20 && r <= 46 && g <= 2 * r - 40 || r > 46 && r <= 80 && g <= -r + 100)
                    {
                        //img_rslt.Data[i, j, 0] = (byte)(((int)255 * b)/100);
                        //img_rslt.Data[i, j, 1] = (byte)(((int)255 * g)/100);
                        //img_rslt.Data[i, j, 2] = (byte)(((int)255 * r)/100);

                        img_rslt.Data[i, j, 0] = 255;
                        img_rslt.Data[i, j, 1] = 255;
                        img_rslt.Data[i, j, 2] = 255;

                    }
                    else
                    {


                        img_rslt.Data[i, j, 0] = 0;
                        img_rslt.Data[i, j, 1] = 0;
                        img_rslt.Data[i, j, 2] = 0;



                    }

                }
            }

            imageBox1.Image = img1;
            imageBox2.Image = img_rslt;
        }






        public void facee()
        {
            //img1 = new Image<Bgr, byte>("D:\\aditya.tif");
            img2 = new Image<Bgr, byte>(img1.Size);
            for (int i = 0; i < img2.Height; i++)
            {
                for (int j = 0; j < img2.Width; j++)
                {
                    img2.Data[i, j, 0] = img1.Data[i, j, 0];
                    img2.Data[i, j, 1] = img1.Data[i, j, 1];
                    img2.Data[i, j, 2] = img1.Data[i, j, 2];

                }


            }
            Image<Gray, byte> img_rslt = new Image<Gray, byte>(img1.Size);
            Image<Gray, byte> gr_img = img1.Convert<Gray, byte>();

            haar = new HaarCascade("haarcascade_frontalface_default.xml");
            haar2 = new HaarCascade("Mouth.xml");

            var facees = gr_img.DetectHaarCascade(haar, 1.4, 4, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(64, 64))[0];
            var mouths = gr_img.DetectHaarCascade(haar2, 1.4, 4, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(15, 15))[0];
            if (facees.Length > 0)
            {  //var facees = gr_img.DetectHaarCascade(haar, 1.4, 4, , new Size(25, 25))[0];

                int g = gr_img.Height;
                int g1 = gr_img.Width;
                System.Diagnostics.Debug.WriteLine(g);
                System.Diagnostics.Debug.WriteLine(g1);
                Bitmap bt = gr_img.ToBitmap();
                Bitmap extract;

                Graphics fcn;
                arr = new Bitmap[facees.Length];
                index = 0;
                foreach (var face in facees)
                {
                    img2.Draw(face.rect, new Bgr(Color.Yellow), 3);

                    extract = new Bitmap(face.rect.Width, face.rect.Height);
                    fcn = Graphics.FromImage(extract);
                    fcn.DrawImage(bt, 0, 0, face.rect, GraphicsUnit.Pixel);
                    arr[index] = extract;
                    index++;

                }
                if (ch == 0)
                { pictureBox1.Image = arr[0]; }
                else
                {
                    pictureBox3.Image = arr[0];

                }
                foreach (var mouth in mouths)
                {
                    //mouth;
                    img2.Draw(mouth.rect, new Bgr(Color.Orange), 1);

                }

                if (ch == 0)
                {
                    imageBox1.Image = img1;

                    imageBox2.Image = img2;
                }
                else
                {

                    imageBox3.Image = img1;

                    imageBox4.Image = img2;
                }
            }
        }


        public void LBP()
        {
            int[] ar;
            ar = new int[8];
            int c = 1;
            double y = 0.2;
            int mx = 255;
            int mn = 0;
            // int[][] histo;


            //Drawing PatternFace first
            System.Diagnostics.Debug.WriteLine("Hello");
            // textBox1.Text = (string)arr[0].Size;
            // pictureBox2.Image = arr[0];



            Image<Gray, Byte> imag2 = new Image<Gray, Byte>(arr[0]);


            Image<Gray, Byte> imag22 = new Image<Gray, Byte>(arr[0]);

            Image<Gray, Byte> imag = new Image<Gray, Byte>(imag2.Size);
            for (int i = 0; i < imag2.Height; i++)
            {
                for (int j = 0; j < imag2.Width; j++)//1. gamma preprocessing
                {
                    imag2.Data[i, j, 0] = (byte)((int)((Math.Pow(((c * (double)(imag2.Data[i, j, 0]) / 255)), y) * 255)));
                }
            }

            imag2._SmoothGaussian(3, 3, 1.0, 2.0); // 2. Gauss preprocessing

            /* for (int i = 0; i < imag2.Height; i++) // 3.contrast preprocessing
             {
                 for (int j = 0; j < imag2.Width; j++)
                 {
                     imag2.Data[i, j, 0] = (byte)(((imag2.Data[i, j, 0] - mn) * 255) / (mx - mn));
                 }
             }*/



            int x = 0;

            for (int i = 1; i < imag2.Height - 1; i++)
            {
                for (int j = 1; j < imag2.Width - 1; j++)
                {
                    x = 0;

                    //1.
                    if (imag2.Data[i, j, 0] > imag2.Data[i, j - 1, 0])//lighter than center
                    {
                        ar[0] = 1;

                        x = x + (int)Math.Pow(2, 7 * ar[0]);




                    }
                    else
                    {

                        ar[0] = 0;
                    }



                    //2.
                    if (imag2.Data[i, j, 0] > imag2.Data[i + 1, j - 1, 0])//lighter than center
                    {
                        ar[1] = 1;
                        x = x + (int)Math.Pow(2, 6 * ar[1]);

                    }
                    else
                    {

                        ar[1] = 0;
                    }

                    //3.

                    if (imag2.Data[i, j, 0] > imag2.Data[i + 1, j, 0])//lighter than center
                    {
                        ar[2] = 1;
                        x = x + (int)Math.Pow(2, 5 * ar[2]);

                    }
                    else
                    {

                        ar[2] = 0;
                    }

                    //4.
                    if (imag2.Data[i, j, 0] > imag2.Data[i + 1, j + 1, 0])//lighter than center
                    {
                        ar[3] = 1;
                        x = x + (int)Math.Pow(2, 4 * ar[3]);

                    }
                    else
                    {

                        ar[3] = 0;
                    }

                    //5.
                    if (imag2.Data[i, j, 0] > imag2.Data[i, j + 1, 0])//lighter than center
                    {
                        ar[4] = 1;
                        x = x + (int)Math.Pow(2, 3 * ar[4]);

                    }
                    else
                    {

                        ar[4] = 0;
                    }

                    //6.
                    if (imag2.Data[i, j, 0] > imag2.Data[i - 1, j + 1, 0])//lighter than center
                    {
                        ar[5] = 1;
                        x = x + (int)Math.Pow(2, 2 * ar[5]);

                    }
                    else
                    {

                        ar[5] = 0;
                    }

                    //7.
                    if (imag2.Data[i, j, 0] > imag2.Data[i - 1, j, 0])//lighter than center
                    {
                        ar[6] = 1;
                        x = x + (int)Math.Pow(2, 1 * ar[6]);

                    }
                    else
                    {

                        ar[6] = 0;
                    }

                    //8.
                    if (imag2.Data[i, j, 0] > imag2.Data[i - 1, j - 1, 0])//lighter than center
                    {
                        ar[7] = 1;
                        x = x + 1;
                    }
                    else
                    {

                        ar[7] = 0;
                    }











                    imag.Data[i, j, 0] = (byte)x;
                    //dividing imag in 8 parts







                }

            }
            if (ch == 0)
            {
                pictureBox2.Image = imag.ToBitmap(imag2.Width, imag2.Height);
            }
            else
            {

                pictureBox4.Image = imag.ToBitmap(imag2.Width, imag2.Height);
            }
            int patch_ht = imag2.Height / 8;
            int patch_wd = imag2.Width / 8;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 128; j++)
                {
                    if (ch == 0)
                    {
                        histo[i, j] = 0;
                    }
                    else
                    {
                        histo2[i, j] = 0;
                    }
                }

            }

            for (int k = 1; k < 8; k++)
            {
                for (int i = 1; i < patch_ht; i++)
                {
                    for (int j = 1; j < patch_wd; j++)
                    {
                        if (ch == 0)
                        {
                            histo[k - 1, imag.Data[i * k - 1, j * k - 1, 0] / 2] += 1;
                            // textBox2.Text = ch.ToString();
                            //  histo2[k - 1, imag.Data[i * k - 1, j * k - 1, 0] / 2] += 1;
                            int a;
                            a = histo[6, 48];
                            textBox2.Text = a.ToString();

                        }
                        else
                        {
                            // textBox2.Text = ch.ToString();

                            histo2[k - 1, imag.Data[i * k - 1, j * k - 1, 0] / 2] += 1;


                        }

                        if (histo2[k - 1, imag.Data[i * k - 1, j * k - 1, 0] / 2] != histo[k - 1, imag.Data[i * k - 1, j * k - 1, 0] / 2])
                        {
                            int a;
                            a = histo[6, 48];
                            // textBox2.Text = a.ToString();
                        }

                    }


                }


            }


            for (int i = patch_ht * 7; i < imag.Height; i++)
            {
                for (int j = patch_wd * 7; j < imag.Width; j++)
                {
                    if (ch == 0)
                    {
                        histo[7, imag.Data[i, j, 0] / 2] += 1;
                        // histo2[7, imag.Data[i, j, 0] / 2] += 1;
                    }
                    else
                    {

                        histo2[7, imag.Data[i, j, 0] / 2] += 1;
                    }

                }


            }

            if (ch == 1)
            {
                int D = 0;

                for (int j = 0; j < 8; j++)
                {
                    for (int i = 0; i < 128; i++)
                    {
                        if ((histo2[j, i] + histo[j, i]) == 0)
                        {
                            D += 0;
                        }
                        else
                        {
                            D += (int)(((histo2[j, i] - histo[j, i]) * (histo2[j, i] - histo[j, i])) / (histo2[j, i] + histo[j, i]));
                            // D+=(int)(Math.Sqrt(histo2[j,i] - histo[j,i]));

                        }
                    }

                }


                textBox1.Text = D.ToString();
                textBox1.Text = " They are different persons.";
                System.Diagnostics.Debug.WriteLine(D);
                if (D < 500)
                {
                    textBox1.Text = "Yes they are same persons , you even don't know it.  How stupid are you ?";
                }
            }



        }



       // private void button1_Click(object sender, EventArgs e)
  /*  public void click()    
    {
            while (ch < 2)
            {
                //if (openFileDialog1.ShowDialog() == DialogResult.OK)
                //{
                string filename;
                if (ch == 0)
                {
                    filename = "D:\\katrina7.tif";

                    img1 = new Image<Bgr, byte>(filename);
                    imageBox1.Image = img1;
                }
                else
                {
                    filename = "D:\\katrina5.tif";

                    img1 = new Image<Bgr, byte>(filename);
                    imageBox3.Image = img1;
                }




                //filename = openFileDialog1.FileName;


                facee();
                textBox1.Text = "Calling LBP";
                LBP();
                //}
                ch++;

            }
        }
        */


        private void next_Click(object sender, EventArgs e)
        {
            if (index >= arr.Length)
            {
                index = 0;
            }

            pictureBox1.Image = arr[index];
            index++;


        }

        private void prev_Click(object sender, EventArgs e)
        {
            if (index <= 0)
            {
                index = arr.Length - 1;
            }

            pictureBox1.Image = arr[index];
            index--;
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            while (ch < 2)
            {
                //if (openFileDialog1.ShowDialog() == DialogResult.OK)
                //{
                string filename;
                if (ch == 0)
                {
                    filename = "D:\\katrina7.tif";

                    img1 = new Image<Bgr, byte>(filename);
                    imageBox1.Image = img1;
                }
                else
                {
                    filename = "D:\\katrina5.tif";

                    img1 = new Image<Bgr, byte>(filename);
                    imageBox3.Image = img1;
                }




                //filename = openFileDialog1.FileName;


                facee();
                textBox1.Text = "Calling LBP";
                LBP();
                //}
                ch++;

            }
        }


        private Image<Gray, byte> Contourfinding(Image<Gray, byte> img)
        {

            Image<Gray, byte> r = new Image<Gray, byte>(img.Size);

            using (MemStorage storage = new MemStorage())
            {

                for (Contour<Point> contours = img.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, storage); contours != null; contours = contours.HNext)
                {


                    Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.015, storage);

                    if (currentContour.BoundingRectangle.Width > 20)
                    {

                        CvInvoke.cvDrawContours(r, contours, new MCvScalar(255), new MCvScalar(255), -1, 1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
                        r.Draw(currentContour.BoundingRectangle, new Gray(), 1);
                    }

                }

            }
            imageBox2.Image = r;
            return r;

        }


        public void smile()
        {
            Image<Bgr, Byte> b = new Image<Bgr, Byte>("D:\\katrina4.tif");
            Image<Gray, Byte> a = new Image<Gray, Byte>("D:\\katrina4.tif");
            int g = a.Height;
            int g1 = a.Width;
            System.Diagnostics.Debug.WriteLine(g);
            System.Diagnostics.Debug.WriteLine(g1);
            Bitmap bt = a.ToBitmap();
            Bitmap extract;
            Bitmap[] arr;
            Graphics fcn;
            int sum = 0;
            int tot = 0;
            int max = 0;
            int min = 255;

            haar2 = new HaarCascade("Mouth.xml");
            var mouths = a.DetectHaarCascade(haar2, 1.4, 4, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(15, 15))[0];
            arr = new Bitmap[mouths.Length];
            index = 0;



            for (int i = 0; i < a.Height; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    if (max < (byte)a.Data[i, j, 0])
                    {
                        max = (byte)a.Data[i, j, 0];
                    }
                }
            }

            for (int i = 0; i < a.Height; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    if (min > (byte)a.Data[i, j, 0])
                    {
                        min = (byte)a.Data[i, j, 0];
                    }
                }
            }


            for (int i = 0; i < a.Height; i++)
            {
                for (int j = 0; j < a.Width; j++)
                {
                    a.Data[i, j, 0] = (byte)(((a.Data[i, j, 0] - min) * 255) / (max - min));
                }
            }










            if (mouths.Length > 0)
            {  //var facees = gr_img.DetectHaarCascade(haar, 1.4, 4, , new Size(25, 25))[0];

                g = a.Height;
                g1 = a.Width;
                System.Diagnostics.Debug.WriteLine(g);
                System.Diagnostics.Debug.WriteLine(g1);
                bt = a.ToBitmap();



                arr = new Bitmap[mouths.Length];
                index = 0;
                foreach (var mth in mouths)
                {
                    a.Draw(mth.rect, new Gray(), 3);

                    extract = new Bitmap(mth.rect.Width, mth.rect.Height);
                    fcn = Graphics.FromImage(extract);
                    fcn.DrawImage(bt, 0, 0, mth.rect, GraphicsUnit.Pixel);
                    arr[index] = extract;
                    index++;

                }


                pictureBox3.Image = arr[0];
                pictureBox2.Image = a.Bitmap;
                pictureBox1.Image = b.Bitmap;





                Image<Gray, Byte> img = new Image<Gray, Byte>(arr[0]);
                sum = 0;
                tot = 0;
                for (int i = 0; i < img.Height; i++)
                {
                    for (int j = 0; j < img.Width; j++)
                    {

                        if (img.Data[i, j, 0] > 200)
                        {

                            sum += 1;
                            //img.Data[i, j, 0] = 255;

                        }
                        else
                        {
                            //img.Data[i, j, 0] = 0;
                        }
                        tot++;
                    }
                }


                double va = sum * 100 / tot;
                textBox1.Text = sum.ToString();
                textBox2.Text = va.ToString();


                imageBox1.Image = img;
                Contourfinding(img);

                if (va > 10)
                {

                    textBox1.Text = "Smile detected";
                }
                else
                {
                    textBox1.Text = " Smile not detected";

                }

            }






        }


















        public void findkey_points(Image<Gray, Byte> im, Image<Gray, Byte> im1, Image<Gray, Byte> im2)
        {

            key = new int[100, 2];
            int xh = 0;
            // xh = globe;
            int k;
            if (ch1 == xh)
            {
                k = 0;
                xh++;
            }
            else
            {
                k = bi;

            }
            System.Diagnostics.Debug.WriteLine("yes");

            // System.Diagnostics.Debug.WriteLine("yes");
            // System.Diagnostics.Debug.WriteLine(mag[k]);

            for (int i = 2; i < im.Height - 2; i++)
            {
                for (int j = 2; j < im.Width - 2; j++)
                {

                    //comparing with same level
                    if (im.Data[i, j, 0] < im.Data[i - 1, j - 1, 0] && im.Data[i, j, 0] < im.Data[i - 1, j, 0] && im.Data[i, j, 0] < im.Data[i - 1, j + 1, 0]
                   && im.Data[i, j, 0] < im.Data[i, j - 1, 0] && im.Data[i, j, 0] < im.Data[i, j + 1, 0] && im.Data[i, j, 0] < im.Data[i + 1, j - 1, 0]
                     && im.Data[i, j, 0] < im.Data[i + 1, j, 0] && im.Data[i, j, 0] < im.Data[i + 1, j + 1, 0])
                    {
                        // System.Diagnostics.Debug.WriteLine("In if 1st");
                        if (im.Data[i, j, 0] <= im1.Data[i - 1, j - 1, 0] && im.Data[i, j, 0] <= im1.Data[i - 1, j, 0] && im.Data[i, j, 0] <= im1.Data[i - 1, j + 1, 0]
                  && im.Data[i, j, 0] <= im1.Data[i, j - 1, 0] && im.Data[i, j, 0] <= im1.Data[i, j + 1, 0] && im.Data[i, j, 0] <= im1.Data[i + 1, j - 1, 0]
                    && im.Data[i, j, 0] <= im1.Data[i + 1, j, 0] && im.Data[i, j, 0] <= im1.Data[i + 1, j + 1, 0] && im.Data[i, j, 0] <= im1.Data[i, j, 0])
                        {


                            if (im.Data[i, j, 0] <= im2.Data[i - 1, j - 1, 0] && im.Data[i, j, 0] <= im2.Data[i - 1, j, 0] && im.Data[i, j, 0] <= im2.Data[i - 1, j + 1, 0]
                  && im.Data[i, j, 0] <= im2.Data[i, j - 1, 0] && im.Data[i, j, 0] <= im2.Data[i, j + 1, 0] && im.Data[i, j, 0] <= im2.Data[i + 1, j - 1, 0]
                    && im.Data[i, j, 0] <= im2.Data[i + 1, j, 0] && im.Data[i, j, 0] <= im2.Data[i + 1, j + 1, 0] && im.Data[i, j, 0] <= im2.Data[i, j, 0])
                            {

                                key[k, 0] = i;
                                key[k, 1] = j;
                                mag[k] = im.Data[i, j, 0];//(int)(Math.Sqrt(Math.Pow(im.Data[i + 1, j, 0] - im.Data[i - 1, j, 0], 2.0) + Math.Pow(im.Data[i, j + 1, 0] - im.Data[i, j - 1, 0], 2.0)));
                                if (mag[k] >= 0)
                                {
                                    System.Diagnostics.Debug.WriteLine("yes11");

                                    // System.Diagnostics.Debug.WriteLine("yes");
                                    // System.Diagnostics.Debug.WriteLine(mag[k]);
                                }

                                if ((im.Data[i + 1, j, 0] - im.Data[i - 1, j, 0]) == 0)
                                {
                                    tan[k++] = 6.28;
                                }
                                else
                                {
                                    tan[k++] = Math.Atan((im.Data[i, j + 1, 0] - im.Data[i, j - 1, 0]) / (im.Data[i + 1, j, 0] - im.Data[i - 1, j, 0]));

                                }

                            }
                        }
                    }




                    if (im.Data[i, j, 0] > im.Data[i - 1, j - 1, 0] && im.Data[i, j, 0] > im.Data[i - 1, j, 0] && im.Data[i, j, 0] > im.Data[i - 1, j + 1, 0]
               && im.Data[i, j, 0] > im.Data[i, j - 1, 0] && im.Data[i, j, 0] > im.Data[i, j + 1, 0] && im.Data[i, j, 0] > im.Data[i + 1, j - 1, 0]
                 && im.Data[i, j, 0] > im.Data[i + 1, j, 0] && im.Data[i, j, 0] > im.Data[i + 1, j + 1, 0])
                    {

                        if (im.Data[i, j, 0] >= im1.Data[i - 1, j - 1, 0] && im.Data[i, j, 0] >= im1.Data[i - 1, j, 0] && im.Data[i, j, 0] >= im1.Data[i - 1, j + 1, 0]
                  && im.Data[i, j, 0] >= im1.Data[i, j - 1, 0] && im.Data[i, j, 0] >= im1.Data[i, j + 1, 0] && im.Data[i, j, 0] >= im1.Data[i + 1, j - 1, 0]
                    && im.Data[i, j, 0] >= im1.Data[i + 1, j, 0] && im.Data[i, j, 0] >= im1.Data[i + 1, j + 1, 0] && im.Data[i, j, 0] >= im1.Data[i, j, 0])
                        {

                            if (im.Data[i, j, 0] >= im2.Data[i - 1, j - 1, 0] && im.Data[i, j, 0] >= im2.Data[i - 1, j, 0] && im.Data[i, j, 0] >= im2.Data[i - 1, j + 1, 0]
                  && im.Data[i, j, 0] >= im2.Data[i, j - 1, 0] && im.Data[i, j, 0] >= im2.Data[i, j + 1, 0] && im.Data[i, j, 0] >= im2.Data[i + 1, j - 1, 0]
                    && im.Data[i, j, 0] >= im2.Data[i + 1, j, 0] && im.Data[i, j, 0] >= im2.Data[i + 1, j + 1, 0] && im.Data[i, j, 0] >= im2.Data[i, j, 0])
                            {

                                key[k, 0] = i;
                                key[k, 1] = j;
                                //mag[k] = (int)(Math.Sqrt(Math.Pow(im.Data[i + 1, j, 0] - im.Data[i - 1, j, 0], 2.0) + Math.Pow(im.Data[i, j + 1, 0] - im.Data[i, j - 1, 0], 2.0)));
                                mag[k] = im.Data[i, j, 0];
                                if (mag[k] >= 0)
                                {
                                    System.Diagnostics.Debug.WriteLine("yes1111");

                                    // System.Diagnostics.Debug.WriteLine("yes");
                                    // System.Diagnostics.Debug.WriteLine(mag[k]);
                                }
                                if ((im.Data[i + 1, j, 0] - im.Data[i - 1, j, 0]) == 0)
                                {
                                    tan[k++] = 6.28;
                                }
                                else
                                {
                                    tan[k++] = Math.Atan((im.Data[i, j + 1, 0] - im.Data[i, j - 1, 0]) / (im.Data[i + 1, j, 0] - im.Data[i - 1, j, 0]));
                                }
                            }
                        }
                    }
                    if (k > 99)
                    {
                        break;
                    }
                }

                if (k > 99)
                {
                    break;
                }

            }

            bi = k;

        }



        public void SIFT_help()
        {
            while (ch1 < 2)
            {
                globe = 0;
                System.Diagnostics.Debug.WriteLine("ch1");
                System.Diagnostics.Debug.WriteLine(ch1);
                if (ch1 == 0)
                {
                    img1 = new Image<Bgr, byte>("D:\\katrina8.tif");
                }
                else
                {

                    img1 = new Image<Bgr, byte>("D:\\katrina7.tif");
                }

                img2 = new Image<Bgr, byte>(img1.Size);
                for (int i = 0; i < img2.Height; i++)
                {
                    for (int j = 0; j < img2.Width; j++)
                    {
                        img2.Data[i, j, 0] = img1.Data[i, j, 0];
                        img2.Data[i, j, 1] = img1.Data[i, j, 1];
                        img2.Data[i, j, 2] = img1.Data[i, j, 2];

                    }


                }
                Image<Gray, byte> img_rslt = new Image<Gray, byte>(img1.Size);
                Image<Gray, byte> gr_img = img1.Convert<Gray, byte>();

                haar = new HaarCascade("haarcascade_frontalface_default.xml");
                //haar2 = new HaarCascade("Mouth.xml");

                var facees = gr_img.DetectHaarCascade(haar, 1.4, 4, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(64, 64))[0];
                // var mouths = gr_img.DetectHaarCascade(haar2, 1.4, 4, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(15, 15))[0];
                if (facees.Length > 0)
                {  //var facees = gr_img.DetectHaarCascade(haar, 1.4, 4, , new Size(25, 25))[0];

                    int g = gr_img.Height;
                    int g1 = gr_img.Width;
                    // System.Diagnostics.Debug.WriteLine(g);
                    // System.Diagnostics.Debug.WriteLine(g1);
                    Bitmap bt = gr_img.ToBitmap();
                    Bitmap extract;

                    Graphics fcn;
                    arr = new Bitmap[facees.Length];
                    index = 0;
                    foreach (var face in facees)
                    {
                        img2.Draw(face.rect, new Bgr(Color.Yellow), 3);

                        extract = new Bitmap(face.rect.Width, face.rect.Height);
                        fcn = Graphics.FromImage(extract);
                        fcn.DrawImage(bt, 0, 0, face.rect, GraphicsUnit.Pixel);
                        arr[index] = extract;
                        index++;

                    }
                    if (ch1 == 0)
                    { pictureBox1.Image = arr[0]; }
                    else
                    {
                        pictureBox2.Image = arr[0];

                    }


                    System.Diagnostics.Debug.WriteLine("Here I am ");
                }

                SIFT();
                ch1++;
            }
        }



        public void SIFT()
        {


            Image<Gray, Byte> imag2 = new Image<Gray, Byte>(arr[0]);
            Image<Gray, Byte> imag3 = new Image<Gray, Byte>(imag2.Size);
            Image<Gray, Byte> imag4 = new Image<Gray, Byte>(imag2.Size);
            Image<Gray, Byte> imag5 = new Image<Gray, Byte>(imag2.Size);
            Image<Gray, Byte> imag6 = new Image<Gray, Byte>(imag2.Size);
            Image<Gray, Byte> imag7 = new Image<Gray, Byte>(imag2.Size);
            Image<Gray, Byte> imag8 = new Image<Gray, Byte>(imag2.Size);
            Image<Gray, Byte> imag9 = new Image<Gray, Byte>(imag2.Size);
            Image<Gray, Byte> imag10 = new Image<Gray, Byte>(imag2.Size);

            imag3 = imag2.SmoothGaussian(3, 3, 10, 10);
            imag4 = imag3.SmoothGaussian(3, 3, 10, 10);
            imag5 = imag4.SmoothGaussian(3, 3, 10, 10);
            imag6 = imag5.SmoothGaussian(3, 3, 10, 10);


            imag7 = imag2 - imag3;
            imag8 = imag3 - imag4;
            imag9 = imag4 - imag5;
            imag10 = imag5 - imag6;

            //pictureBox1.Image = arr[0];
            imageBox1.Image = imag10;
            findkey_points(imag8, imag7, imag9);
            findkey_points(imag9, imag8, imag10);
            System.Diagnostics.Debug.WriteLine("Hello");
            System.Diagnostics.Debug.WriteLine(imag8.Data[10, 11, 0]);
            System.Diagnostics.Debug.WriteLine(mag[2]);
            System.Diagnostics.Debug.WriteLine("Hello1");

            int patch_ht = imag2.Height / 8;
            int patch_wd = imag2.Width / 8;
            for (int k = 1; k <= 7; k++)
            {
                for (int i = 1; i < k * patch_ht; i++)
                {
                    for (int j = 1; j < k * patch_wd; j++)
                    {

                        for (int h = 0; h < bi; h++)
                        {
                            if (key[h, 0] == i)
                            {

                                if (key[h, 1] == j)
                                {

                                    if (tan[h] < 0.785)
                                    {
                                        if (ch1 == 0)
                                        {
                                            histogram1[k - 1, 0] += mag[h];
                                        }
                                        else
                                        {
                                            histogram12[k - 1, 0] += mag[h];
                                        }
                                    }
                                    else if (tan[h] < 1.57)
                                    {
                                        if (ch1 == 0)
                                        {
                                            histogram1[k - 1, 1] += mag[h];
                                        }
                                        else
                                        {
                                            histogram12[k - 1, 1] += mag[h];
                                        }
                                    }
                                    else if (tan[h] < 2.355)
                                    {
                                        if (ch1 == 0)
                                        {
                                            histogram1[k - 1, 2] += mag[h];
                                        }
                                        else
                                        {
                                            histogram12[k - 1, 2] += mag[h];
                                        }
                                    }
                                    else if (tan[h] < 3.14)
                                    {
                                        if (ch1 == 0)
                                        {
                                            histogram1[k - 1, 3] += mag[h];
                                        }
                                        else
                                        {
                                            histogram12[k - 1, 3] += mag[h];
                                        }
                                    }
                                    else if (tan[h] < 3.925)
                                    {
                                        if (ch1 == 0)
                                        {
                                            histogram1[k - 1, 4] += mag[h];
                                        }
                                        else
                                        {
                                            histogram12[k - 1, 4] += mag[h];
                                        }
                                    }
                                    else if (tan[h] < 4.71)
                                    {
                                        if (ch1 == 0)
                                        {
                                            histogram1[k - 1, 5] += mag[h];
                                        }
                                        else
                                        {
                                            histogram12[k - 1, 5] += mag[h];
                                        }
                                    }
                                    else if (tan[h] < 5.495)
                                    {
                                        if (ch1 == 0)
                                        {
                                            histogram1[k - 1, 6] += mag[h];
                                        }
                                        else
                                        {
                                            histogram12[k - 1, 6] += mag[h];
                                        }
                                    }
                                    else if (tan[h] < 6.28)
                                    {
                                        if (ch1 == 0)
                                        {
                                            histogram1[k - 1, 7] += mag[h];
                                        }
                                        else
                                        {
                                            histogram12[k - 1, 7] += mag[h];
                                        }
                                    }

                                }


                            }


                        }
                    }

                }
            }

            for (int i = patch_ht * 7; i < imag2.Height; i++)
            {
                for (int j = patch_wd * 7; j < imag2.Width; j++)
                {
                    for (int h = 0; h < bi; h++)
                    {
                        if (key[h, 0] == i)
                        {

                            if (key[h, 1] == j)
                            {

                                if (tan[h] < 0.785)
                                {
                                    if (ch1 == 0)
                                    {
                                        histogram1[7, 0] += mag[h];
                                    }
                                    else
                                    {
                                        histogram12[7, 0] += mag[h];
                                    }
                                }
                                else if (tan[h] < 1.57)
                                {
                                    if (ch1 == 0)
                                    {
                                        histogram1[7, 1] += mag[h];
                                    }
                                    else
                                    {
                                        histogram12[7, 1] += mag[h];
                                    }
                                }
                                else if (tan[h] < 2.355)
                                {
                                    if (ch1 == 0)
                                    {
                                        histogram1[7, 2] += mag[h];
                                    }
                                    else
                                    {
                                        histogram12[7, 2] += mag[h];
                                    }
                                }
                                else if (tan[h] < 3.14)
                                {
                                    if (ch1 == 0)
                                    {
                                        histogram1[7, 3] += mag[h];
                                    }
                                    else
                                    {
                                        histogram12[7, 3] += mag[h];
                                    }
                                }
                                else if (tan[h] < 3.925)
                                {
                                    if (ch1 == 0)
                                    {
                                        histogram1[7, 4] += mag[h];
                                    }
                                    else
                                    {
                                        histogram12[7, 4] += mag[h];
                                    }
                                }
                                else if (tan[h] < 4.71)
                                {
                                    if (ch1 == 0)
                                    {
                                        histogram1[7, 5] += mag[h];
                                    }
                                    else
                                    {
                                        histogram12[7, 5] += mag[h];
                                    }
                                }
                                else if (tan[h] < 5.495)
                                {
                                    if (ch1 == 0)
                                    {
                                        histogram1[7, 6] += mag[h];
                                    }
                                    else
                                    {
                                        histogram12[7, 6] += mag[h];
                                    }
                                }
                                else if (tan[h] < 6.28)
                                {
                                    if (ch1 == 0)
                                    {
                                        histogram1[7, 7] += mag[h];
                                    }
                                    else
                                    {
                                        histogram12[7, 7] += mag[h];
                                    }
                                }

                            }


                        }


                    }
                }

            }
            int D = 0;
            System.Diagnostics.Debug.WriteLine("Nearing End");
            if (ch1 != 0)
            {

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        D += (int)(Math.Sqrt(Math.Abs((histogram1[i, j] - histogram12[i, j]) * (histogram1[i, j] - histogram12[i, j]))));
                    }
                }
            }


            // textBox1.Text =(histogram12[2,7]).ToString();
            if (D < 25)
            {
                textBox1.Text = "Same persons";
            }
            else 
            {
                 textBox1.Text ="Not Same persons";
            }
            System.Diagnostics.Debug.WriteLine("Yahan ke baad kahan");
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            Application.Idle += new EventHandler(myfunc);
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            try
            {

                MessageBox.Show(textBox1.Text + "entered into button 2 click try", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CountTrain = CountTrain + 1;
                //Here change pls
                Image<Gray, byte> img1 = new Image<Gray, byte>("D:\\katrina6.tif");
                mygray = img1.Convert<Gray, byte>();

                MCvAvgComp[][] facesDetected = mygray.DetectHaarCascade(
                myface,
                1.4,
                4,
                HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));



                MessageBox.Show(textBox1.Text + "before for loop", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                foreach (MCvAvgComp iter in facesDetected[0])
                {
                    Total_TrainedFace = mygray.Copy(iter.rect).Convert<Gray, byte>();
                    break;
                }

                Total_TrainedFace = myresult.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                mytrainingImages.Add(Total_TrainedFace);
                mylabels.Add(textBox1.Text);

                imageBox1.Image = mygray;

                File.WriteAllText(Application.StartupPath + "/TrainedFaces_folder/TrainedLabels_file.txt", mytrainingImages.ToArray().Length.ToString() + "%");

                for (int i = 1; i < mytrainingImages.ToArray().Length + 1; i++)
                {
                    mytrainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces_folder/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces_folder/TrainedLabels_file.txt", mylabels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(textBox1.Text + "´s face has been detected & added successfully", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("First click the detection button", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }




        void myfunc(object sender, EventArgs e)
        {
            label1.Text = "0";

            all_Persons.Add("");
            //Here change pls

            Image<Bgr, byte> img1 = new Image<Bgr, byte>("D:\\katrina6.tif");


            mygray = img1.Convert<Gray, byte>();

            MCvAvgComp[][] facesDetected = mygray.DetectHaarCascade(myface, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));


            foreach (MCvAvgComp iter in facesDetected[0])
            {
                t_var = t_var + 1;
                myresult = mygray.Copy(iter.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                img1.Draw(iter.rect, new Bgr(Color.Red), 2);


                if (mytrainingImages.ToArray().Length != 0)
                {

                    MCvTermCriteria term_Criteriaeria = new MCvTermCriteria(CountTrain, 0.001);


                    EigenObjectRecognizer myrecognizer = new EigenObjectRecognizer(mytrainingImages.ToArray(), mylabels.ToArray(), 3000, ref term_Criteriaeria);

                    his_name = myrecognizer.Re_cognize(myresult);


                    img1.Draw(his_name, ref myfont, new Point(iter.rect.X - 2, iter.rect.Y - 2), new Bgr(Color.LightGreen));

                }

                all_Persons[t_var - 1] = his_name;
                all_Persons.Add("");



                label1.Text = facesDetected[0].Length.ToString();


            }
            t_var = 0;


            for (int j = 0; j < facesDetected[0].Length; j++)
            {
                all_names = all_names + all_Persons[j] + ", ";
            }

            imageBox3.Image = mygray;
            label2.Text = all_names;
            all_names = "";

            all_Persons.Clear();

        }



        public class EigenObjectRecognizer
        {
            private Image<Gray, Single>[] eigen_Images;
            private Image<Gray, Single> avg_Image;
            private Matrix<float>[] eigen_Values;
            private string[] label_s;
            private double eigen_DistanceThreshold;

            public Image<Gray, Single>[] Eigen_Images
            {
                get { return eigen_Images; }
                set { eigen_Images = value; }
            }

            public String[] Label_s
            {
                get { return label_s; }
                set { label_s = value; }
            }

            public double EigenDistance_Threshold
            {
                get { return eigen_DistanceThreshold; }
                set { eigen_DistanceThreshold = value; }
            }

            public Image<Gray, Single> Average_Image
            {
                get { return avg_Image; }
                set { avg_Image = value; }
            }

            public Matrix<float>[] Eigen_Values
            {
                get { return eigen_Values; }
                set { eigen_Values = value; }
            }

            private EigenObjectRecognizer()
            {
            }


            public EigenObjectRecognizer(Image<Gray, Byte>[] images, ref MCvTermCriteria term_Criteria)
                : this(images, Generate_Labels(images.Length), ref term_Criteria)
            {
            }

            private static String[] Generate_Labels(int size)
            {
                String[] labels = new string[size];
                for (int i = 0; i < size; i++)
                    labels[i] = i.ToString();
                return labels;
            }

            public EigenObjectRecognizer(Image<Gray, Byte>[] images, String[] labels, ref MCvTermCriteria term_Criteria)
                : this(images, labels, 0, ref term_Criteria)
            {
            }

            public EigenObjectRecognizer(Image<Gray, Byte>[] images, String[] labels, double eigenDistanceThreshold, ref MCvTermCriteria term_Criteria)
            {
                CalcEigen_Objects(images, ref term_Criteria, out eigen_Images, out avg_Image);


                eigen_Values = Array.ConvertAll<Image<Gray, Byte>, Matrix<float>>(images,
                    delegate(Image<Gray, Byte> img)
                    {
                        return new Matrix<float>(Eigen_Decomposite(img, eigen_Images, avg_Image));
                    });

                label_s = labels;

                eigen_DistanceThreshold = eigenDistanceThreshold;
            }


            public static void CalcEigen_Objects(Image<Gray, Byte>[] trainingImages, ref MCvTermCriteria term_Criteria, out Image<Gray, Single>[] eigenImages, out Image<Gray, Single> avg)
            {
                int width = trainingImages[0].Width;
                int height = trainingImages[0].Height;

                IntPtr[] inObjs = Array.ConvertAll<Image<Gray, Byte>, IntPtr>(trainingImages, delegate(Image<Gray, Byte> img) { return img.Ptr; });

                if (term_Criteria.max_iter <= 0 || term_Criteria.max_iter > trainingImages.Length)
                    term_Criteria.max_iter = trainingImages.Length;

                int maxEigenObjs = term_Criteria.max_iter;


                eigenImages = new Image<Gray, float>[maxEigenObjs];
                for (int i = 0; i < eigenImages.Length; i++)
                    eigenImages[i] = new Image<Gray, float>(width, height);
                IntPtr[] eigObjs = Array.ConvertAll<Image<Gray, Single>, IntPtr>(eigenImages, delegate(Image<Gray, Single> img) { return img.Ptr; });


                avg = new Image<Gray, Single>(width, height);

                CvInvoke.cvCalcEigenObjects(
                    inObjs,
                    ref term_Criteria,
                    eigObjs,
                    null,
                    avg.Ptr);
            }

            public static float[] Eigen_Decomposite(Image<Gray, Byte> src, Image<Gray, Single>[] eigenImages, Image<Gray, Single> avg)
            {
                return CvInvoke.cvEigenDecomposite(
                    src.Ptr,
                    Array.ConvertAll<Image<Gray, Single>, IntPtr>(eigenImages, delegate(Image<Gray, Single> img) { return img.Ptr; }),
                    avg.Ptr);
            }


            public Image<Gray, Byte> Eigen_Projection(float[] eigenValue)
            {
                Image<Gray, Byte> res = new Image<Gray, byte>(avg_Image.Width, avg_Image.Height);
                CvInvoke.cvEigenProjection(
                    Array.ConvertAll<Image<Gray, Single>, IntPtr>(eigen_Images, delegate(Image<Gray, Single> img) { return img.Ptr; }),
                    eigenValue,
                    avg_Image.Ptr,
                    res.Ptr);
                return res;
            }

            public float[] GetEigen_Distances(Image<Gray, Byte> image)
            {
                using (Matrix<float> eigenValue = new Matrix<float>(Eigen_Decomposite(image, eigen_Images, avg_Image)))
                    return Array.ConvertAll<Matrix<float>, float>(eigen_Values,
                        delegate(Matrix<float> eigenValueI)
                        {
                            return (float)CvInvoke.cvNorm(eigenValue.Ptr, eigenValueI.Ptr, Emgu.CV.CvEnum.NORM_TYPE.CV_L2, IntPtr.Zero);
                        });
            }

            public void FindMostSimilar_Object(Image<Gray, Byte> image, out int index, out float eigenDistance, out String label)
            {
                float[] dist = GetEigen_Distances(image);

                index = 0;
                eigenDistance = dist[0];
                for (int i = 1; i < dist.Length; i++)
                {
                    if (dist[i] < eigenDistance)
                    {
                        index = i;
                        eigenDistance = dist[i];
                    }
                }
                label = Label_s[index];
            }


            public String Re_cognize(Image<Gray, Byte> image)
            {
                int index;
                float eigenDistance;
                String label;
                FindMostSimilar_Object(image, out index, out eigenDistance, out label);

                return (eigen_DistanceThreshold <= 0 || eigenDistance < eigen_DistanceThreshold) ? label_s[index] : String.Empty;
            }
        }

        private void button5_Click(object sender, System.EventArgs e)
        {
            smile();
        }

        private void button6_Click(object sender, System.EventArgs e)
        {
            face1();
        }

     

        private void button2_Click(object sender, System.EventArgs e)
        {
            SIFT_help();
        }


    }



}
