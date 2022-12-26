using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Un4seen.Bass;
using System.Windows.Forms.DataVisualization.Charting;

namespace spectr_bass
{
    public partial class Form1 : Form
    {
        string audio1 = "", audio2="", audio3 = "";
        PointF p1;
        int counter = 0;
        int k = 0;
        int tick = 0,tick1;
        int chan = 0;//номер потока
        int track_collvo = 0;
        int[] vhod ;
        string[] tracks_names;
        double[] min_mass_coef1 = new double[150];
        Single[] fft = null;//массив данных спектра
      

        Single[] fft_chast = null;
        Single[] fft1_chast = null;
        Single[] fft2_chast = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "*.mp3|*.txt|All files(*.*)|*.*";
            p1 = new PointF(0.0f, 0.0f);
        }
        private void button1_Click(object sender, EventArgs e)
        {
           

        }
        public int Abs(int k)
        {
            if(k < 0)
            {
              
                return k*-1;
            }
            else
            {
                return k;
            }
        }
        public void file_save()
        {
            
            int[] max_mass = new int[k];
            int[] min_mass = new int[k];
            int[] max_mass_filtr = new int[k];
            int[] min_mass_filtr = new int[k];
            double[] min_mass_coef = new double[k];
            // bool rost = false,ubav = false;
            int[] maxold = new int[2]; int max = 0;
            int razn = 0;
            int razn1 = 0;//,razn2 = 0;
            int vhod = 0, vhod1 = 0;
            for (int z = 0; z < 150; z++)
            {

                if (z >= 2)
                {
                    max = (int)fft_chast[z];
                    if ((maxold[0] < maxold[1]) && (maxold[1] > max))
                    {

                        max_mass[z] = maxold[1];


                    }
                    if ((maxold[0] > maxold[1]) && (maxold[1] < max))
                    {

                        min_mass[z] = maxold[1];


                    }
                    maxold[1] = (int)fft_chast[z - 1];
                    maxold[0] = (int)fft_chast[z - 2];

                }
           

            }
            int i = 0, j = 0;
            for (int z = 0; z < k-1; z++)
            {
                if (max_mass[z] > 0 || min_mass[z] > 0)
                {
                    if (max_mass[z] > 0)
                    {
                        max_mass_filtr[i] = max_mass[z];
                        
                        if (max_mass[z + 1] > 0)
                        {
                            max_mass[z + 1] = 0;
                        }
                        i++;
                    }
                    if (min_mass[z] > 0)
                    {
                        min_mass_filtr[j] = min_mass[z];
                        if (min_mass[z + 1] > 0)
                        {
                            min_mass[z + 1] = 0;
                        }
                        j++;
                    }


                }

            }
            for (int z = 0; z < 150; z++)
            {
                if ((max_mass_filtr[z] > 0) && (min_mass_filtr[z] > 0))
                {
                    
                    min_mass_coef[z] = ((double)max_mass_filtr[z] / min_mass_filtr[z]);
                    // Console.WriteLine(max_mass_filtr[z] + " " + min_mass_filtr[z] + " " + min_mass_coef[z]);
                }
            }
            StreamWriter f = new StreamWriter("base.txt", true);

            f.WriteLine(openFileDialog1.FileName);

            for (int q = 0; q < min_mass_coef.Length;q++)
            {
                // byte[] array = System.Text.Encoding.Default.GetBytes(mass[i].ToString());
               // Console.WriteLine(min_mass_coef[q]);
                f.WriteLine(min_mass_coef[q].ToString());
            }
            f.Close();
            Console.WriteLine("я записал");
        }
        public void check(double[] mass)
        {
            int[] max_mass = new int[k];
            int[] min_mass = new int[k];
            int[] max_mass_filtr = new int[k];
            int[] min_mass_filtr = new int[k];
            double[] min_mass_coef = new double[k];
            // bool rost = false,ubav = false;
            int[] maxold = new int[2]; int max = 0;
            int razn = 0;
            int razn1 = 0;//,razn2 = 0;
           
            for (int z = 0; z < 150; z++)
            {

                if (z >= 2)
                {
                    max = (int)fft_chast[z];
                    if ((maxold[0] < maxold[1]) && (maxold[1] > max))
                    {

                        max_mass[z] = maxold[1];


                    }
                    if ((maxold[0] > maxold[1]) && (maxold[1] < max))
                    {

                        min_mass[z] = maxold[1];


                    }
                    maxold[1] = (int)fft_chast[z - 1];
                    maxold[0] = (int)fft_chast[z - 2];

                }


            }
            int i = 0, j = 0;
            for (int z = 0; z < k - 1; z++)
            {
                if (max_mass[z] > 0 || min_mass[z] > 0)
                {
                    if (max_mass[z] > 0)
                    {
                        max_mass_filtr[i] = max_mass[z];

                        if (max_mass[z + 1] > 0)
                        {
                            max_mass[z + 1] = 0;
                        }
                        i++;
                    }
                    if (min_mass[z] > 0)
                    {
                        min_mass_filtr[j] = min_mass[z];
                        if (min_mass[z + 1] > 0)
                        {
                            min_mass[z + 1] = 0;
                        }
                        j++;
                    }


                }

            }
            for (int z = 0; z < 150; z++)
            {
                if ((max_mass_filtr[z] > 0) && (min_mass_filtr[z] > 0))
                {

                    min_mass_coef[z] = ((double)max_mass_filtr[z] / min_mass_filtr[z]);
                    // Console.WriteLine(max_mass_filtr[z] + " " + min_mass_filtr[z] + " " + min_mass_coef[z]);
                }
            }
            for (int z = 0; z < 150; z++)
            {
                if (min_mass_coef[z] > 0)
                {

                    if (((min_mass_coef[z] - mass[z]) <= 0.08) && ((min_mass_coef[z] - mass[z]) >= -0.08))
                    {
                        Console.WriteLine(min_mass_coef[z] + " " + (mass[z]));
                        vhod[counter]++;
                        
                    }
                }
            }
           
            counter++;
            
        }
        void search_max_vhod()
        {
            int max = 0;
            int max_index = 0;
         
            for (int z = 0; z < vhod.Length; z++)
            {
                if(vhod[z] >= max)
                {
                    
                    max = vhod[z];
                    max_index = z;
                }
             
            }
            string str ="",str1 = "",str_contr = "";
            for (int i = tracks_names[max_index].Length-1; i > 0;i--)
            {
                if (tracks_names[max_index][i] == '\\')
                {
                    break;
                  
                }
                else
                {
                    str += tracks_names[max_index][i];
                }
            }
            for (int i = str.Length-1 ; i >= 0; i--)
            {
                str1 += str[i];
            }

            textBox1.Text = "наиболее похожий трек в базе : " + str1;
            str_contr = str1;
            textBox2.Text = "совпаших коэфицентов: " + vhod[max_index];
            vhod[max_index] = 0;
        again:
            str = "";
            str1 = "";
             max = 0;
            max_index = 0;
           
            for (int z = 0; z < vhod.Length; z++)
            {
                if (vhod[z] >= max)
                {

                    max = vhod[z];
                    max_index = z;
                }

            }
           
            for (int i = tracks_names[max_index].Length - 1; i > 0; i--)
            {
                if (tracks_names[max_index][i] == '\\')
                {
                    break;

                }
                else
                {
                    str += tracks_names[max_index][i];
                }
            }
            for (int i = str.Length - 1; i >= 0; i--)
            {
                str1 += str[i];
            }
            Console.WriteLine(str_contr);
            Console.WriteLine(str1);
            if (str_contr == str1)
            {
                vhod[max_index] = 0;
                goto again;
               
            }
            else
            {
                textBox6.Text = "так же похож: " + str1;
            }
            //  Console.WriteLine("track: " + str1 + "  voshlo:  " + vhod[max_index]);
        }
        void tracks_colvo()
        {
            StreamReader f = new StreamReader("base.txt");
            string line = f.ReadLine();
            while (line != null)
            {

                if (line.Length > 1)
                {
                    if (line[2] == '\\')
                    {
                        track_collvo++;
                    }
                }
                line = f.ReadLine();
            }
            f.Close();

        }
        public void file_read()
        {
            tracks_colvo();
            vhod = new int[track_collvo];
            tracks_names = new string[track_collvo];
            int tracks_coll = 0;
            StreamReader f = new StreamReader("base.txt");
            string line = f.ReadLine();
            int i = 0;
            while (line != null)
            {
                
                if (line.Length > 1)
                {
                    if (line[2] == '\\')
                    {
                        if(tracks_coll == 0)
                        {
                            tracks_names[0] = line;
                            Console.WriteLine("track: " + line);
                        }
                        
                        else
                        {
                            check(min_mass_coef1);
                            Console.WriteLine("track: " + line);
                            tracks_names[counter] = line;
                            i = 0;
                        }
                        tracks_coll++;
                    }
                    else
                    {
                        min_mass_coef1[i] = Convert.ToDouble(line);
                        //Console.WriteLine(min_mass_coef1[i]);
                        i++;
                    }
                }
                line = f.ReadLine();
            }
            check(min_mass_coef1);
            
         
            
            f.Close();
            search_max_vhod();
        }
        public void sravn()
        {
            int[] max_mass = new int[k];
            int[] min_mass = new int[k];
            int[] max_mass_filtr = new int[k];
            int[] min_mass_filtr = new int[k];
            double[] min_mass_coef = new double[k];
           // bool rost = false,ubav = false;
            int[] maxold = new int[2];int max = 0;
            int razn = 0;
            int razn1 = 0;//,razn2 = 0;
            int vhod = 0,vhod1 = 0;
          //  int count = 0;
            if (k > 150)
            {
                for (int z = 0; z < 150; z++)
                {
                    razn += (int)((fft_chast[z] - fft1_chast[z])*(fft_chast[z] - fft1_chast[z]));
                    
                }
                for (int z = 0; z < 150; z++)
                {
                    razn1 += (int)((fft_chast[z] - fft2_chast[z]) * (fft_chast[z] - fft2_chast[z]));

                }
                
                
                if(razn < razn1) textBox1.Text =("трек №1 наиболее похож на исходный");
                else textBox1.Text = ("трек №2 наиболее похож на исходный");
                for (int z = 0; z < 150; z++)
                {
                   
                    if (z>=2)
                    {
                        max = (int)fft_chast[z];
                        if((maxold[0] < maxold[1])&&(maxold[1] > max))
                        {
                            
                                max_mass[z] = maxold[1];
                            
                         
                        }
                        if ((maxold[0] > maxold[1]) && (maxold[1] < max))
                        {
                            
                                min_mass[z] = maxold[1];
                            
                          
                        }
                        maxold[1] = (int)fft_chast[z-1];
                        maxold[0] = (int)fft_chast[z-2];
                        
                    }
                   

                }
                int i = 0,j = 0;
                for (int z = 0; z < 150; z++)
                {
                    if (max_mass[z] > 0 || min_mass[z] > 0)
                    {
                        if (max_mass[z] > 0)
                        {
                            max_mass_filtr[i] = max_mass[z];
                           
                            if (max_mass[z + 1] > 0)
                            {
                                max_mass[z + 1] = 0;
                            }
                            i++;
                        }
                        if (min_mass[z] > 0)
                        {
                            min_mass_filtr[j] = min_mass[z];
                           if (min_mass[z + 1] > 0)
                            {
                                min_mass[z + 1] = 0;
                            }
                            j++;
                        }
                       
                        
                    }

                }
                for (int z = 0; z < 150; z++)
                {
                    if ((max_mass_filtr[z] > 0) && (min_mass_filtr[z] > 0))
                    {
                       
                        min_mass_coef[z] = ((double)max_mass_filtr[z] / min_mass_filtr[z]);
                       // Console.WriteLine(max_mass_filtr[z] + " " + min_mass_filtr[z] + " " + min_mass_coef[z]);
                    }
                }
               
                Console.WriteLine("____________");
            


               

                max_mass = new int[k];
                min_mass = new int[k];
                max_mass_filtr = new int[k];
                min_mass_filtr = new int[k];
                for (int z = 0; z < 150; z++)
                {

                    if (z >= 2)
                    {
                        max = (int)fft1_chast[z];
                        if ((maxold[0] < maxold[1]) && (maxold[1] > max))
                        {
                            max_mass[z] = maxold[1];
                          //  max_control2.Points.AddXY(z, max_mass[z] * 10);
                        }
                        if ((maxold[0] > maxold[1]) && (maxold[1] < max) )
                        {
                            min_mass[z] = maxold[1];
                           // max_control2.Points.AddXY(z, min_mass[z] * 10);
                        }
                        maxold[1] = (int)fft1_chast[z - 1];
                        maxold[0] = (int)fft1_chast[z - 2];
                    }

                }
                i = 0;
                j = 0;
                for (int z = 0; z < 150; z++)
                {
                    if ((max_mass[z] > 0) ||( min_mass[z] > 0))
                    {
                        if (max_mass[z] > 0)
                        {

                            max_mass_filtr[i] = max_mass[z];
                           // Console.WriteLine("filtr" + max_mass_filtr[i]);
                            if (max_mass[z + 1] > 0)
                            {
                                max_mass[z + 1] = 0;
                            }
                            i++;
                        }
                        if (min_mass[z] > 0)
                        {
                            min_mass_filtr[j] = min_mass[z];
                            if (min_mass[z + 1] > 0)
                            {
                                min_mass[z + 1] = 0;
                            }
                            j++;
                        }


                    }

                }
                Console.WriteLine("-----------------------");
                for (int z = 0; z < 150; z++)
                {
                    
                    if ((max_mass_filtr[z] > 0 )&&( min_mass_filtr[z] > 0))
                    {
                      
                        if (((min_mass_coef[z] - ((double)max_mass_filtr[z] / min_mass_filtr[z])) <= 0.5) && ((min_mass_coef[z] - ((double)max_mass_filtr[z] / min_mass_filtr[z])) >= -0.5))
                        {
                            Console.WriteLine(min_mass_coef[z] + " " + ((double)max_mass_filtr[z] / min_mass_filtr[z]));
                            vhod++;
                        }
                    
                    }
                }
                Console.WriteLine("vhod: " + vhod);
               
              

                max_mass = new int[k];
                min_mass = new int[k];
                max_mass_filtr = new int[k];
                min_mass_filtr = new int[k];

                for (int z = 0; z < 150; z++)
                {

                    if (z >= 2)
                    {
                        max = (int)fft2_chast[z];
                        if ((maxold[0] < maxold[1]) && (maxold[1] > max))
                        {
                            max_mass[z] = maxold[1];
                          //  max_control3.Points.AddXY(z, max_mass[z] * 10);
                        }
                        if ((maxold[0] > maxold[1]) && (maxold[1] < max))
                        {
                            min_mass[z] = maxold[1];
                           // max_control3.Points.AddXY(z, min_mass[z] * 10);
                        }
                        maxold[1] = (int)fft2_chast[z - 1];
                        maxold[0] = (int)fft2_chast[z - 2];
                    }

                }
                
                i = 0;
                j = 0;
                for (int z = 0; z < 150; z++)
                {
                    if (max_mass[z] > 0 || min_mass[z] > 0)
                    {
                        if (max_mass[z] > 0)
                        {
                            max_mass_filtr[i] = max_mass[z];

                            if (max_mass[z + 1] > 0)
                            {
                                max_mass[z + 1] = 0;
                            }
                            i++;
                        }
                        if (min_mass[z] > 0)
                        {
                            min_mass_filtr[j] = min_mass[z];
                            if (min_mass[z + 1] > 0)
                            {
                                min_mass[z + 1] = 0;
                            }
                            j++;
                        }


                    }

                }
                Console.WriteLine("+++++++++++++++++++");

                for (int z = 0; z < 150; z++)
                {

                    if ((max_mass_filtr[z] > 0)&&(min_mass_filtr[z] > 0))
                    {
                        
                        if ((min_mass_coef[z] == ((double)max_mass_filtr[z] / min_mass_filtr[z]))||((min_mass_coef[z] - ((double)max_mass_filtr[z] / (double)min_mass_filtr[z]))<= 0.5) && ((min_mass_coef[z] - ((double)max_mass_filtr[z] / (double)min_mass_filtr[z])) >= -0.5))
                        {
                            Console.WriteLine(min_mass_coef[z] + " " + ((double)max_mass_filtr[z] / min_mass_filtr[z]));
                            vhod1++;
                        }

                    }
                }
                Console.WriteLine("vhod1: " + vhod1);
              
                
                if (vhod > vhod1) textBox2.Text = ("трек №1 наиболее похож на исходный по мелодии");
                else textBox2.Text = ("трек №2 наиболее похож на исходный по мелодии");
            }//максимальная частота, для которой амплитуда ненулевая
   
        }
        private void timer1_Tick(object sender, EventArgs e)//запускаем каждые 500 мс
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)//построение графика
        {
          
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           // Console.WriteLine(Bass.BASS_GetDeviceInfo(2));
            //Console.WriteLine(Bass.BASS_RecordGetDeviceInfo(0));
            fft_chast = new Single[2048];//выделяем массив для данных          
            fft = new Single[2048];//выделяем массив для данных            
          
            fft1_chast = new Single[2048];
           
            fft2_chast = new Single[2048];
            // Console.WriteLine("sss");
            int n = Bass.BASS_GetDevice();//получаем устройство по умолчанию
            if (Bass.BASS_Init(n, 44100, 0, IntPtr.Zero) == false)
            {
                MessageBox.Show("BASS_Init failed");
                return;
            }
          
            chan = Bass.BASS_StreamCreateFile(audio1,

                0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_SAMPLE_LOOP);//создаем поток в режиме FLOAT

          
            
            if (chan == 0)
            {
                MessageBox.Show("BASS_StreamCreateFile failed");
                return;
            }

            bool f ;
            
            f = Bass.BASS_ChannelPlay(chan, false);//воспроизводим поток
           
      
            if (f == false)
            {
                MessageBox.Show("BASS_ChannelPlay failed");
                return;
            }


            timer2.Start();
            timer1.Start();
            timer3.Start();
            
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            if (Bass.BASS_ChannelIsActive(chan) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                if (fft == null) return;

                PointF p1, p2;
                float max_y = 0;//максимальное значение амплитуды
                float min_y = Single.MaxValue;//минимальное значение амплитуды
                float max_x = fft.Length - 1;//максимальная частота, для которой амплитуда ненулевая

                int i = 0;
                max_x = 0;
                foreach (float f in fft)//находим максимальные и минимальные значения
                {
                    if (f > max_y) max_y = f;
                    if (f < min_y) min_y = f;

                    if (f > 0.001f) max_x = (float)i;


                    i++;
                    //   Console.WriteLine(Bass.BASS_ChannelGetPosition(chan, BASSMode.BASS_POS_BYTES));
                }

                /*меняем направление оси Y, чтобы она смотрела вверх*/
                e.Graphics.ScaleTransform(1.0f, -1.0f);
                e.Graphics.TranslateTransform(0.0f, -1.0f * panel1.Height);

                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

                float y;
                p1 = new PointF(0.0f, 0.0f);//начальная точка

                for (float x = 1; x <= max_x; x++)
                {
                    y = fft[(int)x];

                    /*вычисляем координату следующей точки по относительной амплитуде*/

                    p2 = new PointF((x / max_x) * panel1.Width, panel1.Height * (y - min_y) / (max_y - min_y));

                    path.AddLine(p1, p2);//добавляем линию в график
                                         //  Console.WriteLine(p1.X);

                    p1 = p2;
                }
                e.Graphics.DrawPath(Pens.Black, path);
            }
           
        }
        

      

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] path = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            try
            {
                audio1 = path[0];
                audio1 = path[1];
                audio1 = path[2];
                textBox3.Text = path[0];
              
            }
            catch
            {
                Console.WriteLine(path);
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            file_read();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            if (Bass.BASS_ChannelIsActive(chan) == BASSActive.BASS_ACTIVE_PLAYING)
            {

                float max_y = 0;//максимальное значение амплитуды
                float min_y = Single.MaxValue;//минимальное значение амплитуды
                float max_x = fft.Length - 1;//максимальная частота, для которой амплитуда ненулевая

                int i = 0;
                max_x = 0;

                foreach (float f in fft)//находим максимальные и минимальные значения
                {
                    if (f > max_y) max_y = f;
                    if (f < min_y) min_y = f;

                    if (f > 0.001f)
                    {
                        max_x = (float)i;

                    }
                    // 

                    i++;
                }



                fft_chast[k] = (int)max_x;


                k++;




            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (Bass.BASS_ChannelIsActive(chan) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                if (chan == 0) return;
                if (Bass.BASS_ChannelIsActive(chan) != BASSActive.BASS_ACTIVE_PLAYING) return;
              
                Bass.BASS_ChannelGetData(chan, fft, (int)BASSData.BASS_DATA_FFT4096);//получаем спектр потока
                                                                                     // Console.WriteLine( Bass.BASS_SampleGetData(chan, fft));
                fft[0] = 0.0f;//избавляемся от постоянной составляющей

               
            }
           
            panel1.Refresh();//перерисовка графика
        }

        private void button4_Click(object sender, EventArgs e)
        {
           // if()
            file_save();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            tick1++;

           
                if ((Bass.BASS_ChannelIsActive(chan) == BASSActive.BASS_ACTIVE_PLAYING)&&(tick1 > 150))
                {
                    Bass.BASS_ChannelStop(chan);


                //Console.WriteLine(tick1);
                // file_save();
                
                tick1 = 0;
                    
                }
               

                   


           }

           private void button2_Click(object sender, EventArgs e)
           {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            if(audio1 =="")
            {
                audio1 = openFileDialog1.FileName;
                textBox3.Text = audio1;
            }
            else
            {
                if(audio2 == "")
                {
                    audio2 = openFileDialog1.FileName;
                    
                }
                else
                {
                    if (audio3 == "")
                    {
                        audio3 = openFileDialog1.FileName;
                       
                    }
                }
            }

        }



        

        private void chart1_Click(object sender, EventArgs e)
        {
            //chart1 = new Chart();
            //кладем его на форму и растягиваем на все окно.
            // chart1.Parent = this;
            //chart1.Dock = DockStyle.Fill;

           
           
            // chart1.Series.Clear();
            // mySeriesOfPoint.Points.Clear();
           
        }

       
            
        
    }
}

