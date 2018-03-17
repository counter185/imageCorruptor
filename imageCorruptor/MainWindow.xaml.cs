using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.IO;

namespace imageCorruptor
{
    public partial class MainWindow : Window
    {
        public OpenFileDialog readImg = new OpenFileDialog() {
            Filter = "Image files |*.jpg;*.png",
            Title = "Select Image",
        };

        public SaveFileDialog saveImg = new SaveFileDialog() {
            Filter = "Image files |*.jpg", // don't worry about the filter only being jpg, it changes automatically
            Title = "Save Image",
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Loadimg_click(object sender, RoutedEventArgs e)
        {
            readImg.ShowDialog();
            filepath.Content = readImg.FileName;
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            if (readImg.CheckFileExists && readImg.FileName != String.Empty)
            {
                saveImg.Filter = "Image files |*" + Path.GetExtension(readImg.FileName);
                int i = 0;
                List<byte> buffer = ByteArrayToList(File.ReadAllBytes(readImg.FileName));
                Random rng = new Random();

                for (i = 0; i != int.Parse(Tries.Text); i++)
                {

                    int insertIndex = rng.Next(2, buffer.Count - 1);

                    for (int u = 0; u != int.Parse(Level.Text); u++)
                    {
                        buffer.Insert(insertIndex, byte.Parse(rng.Next(0, 256).ToString()));
                        insertIndex++;
                        insertIndex++;
                    }
                }

                saveImg.ShowDialog();
                if (saveImg.FileName != String.Empty)
                {
                    File.WriteAllBytes(saveImg.FileName, ByteListToArray(buffer));
                }
            }
        }

        private void Tries_TextChanged(object sender, TextChangedEventArgs e)
        {
            SpellCheckNumber(Tries);
        }

        private void Level_TextChanged(object sender, TextChangedEventArgs e)
        {
            SpellCheckNumber(Level);
        }

        //These functions down here were from an external DLL I created
        //I decided against using it when uploading to GitHub

        public static void SpellCheckNumber(System.Windows.Controls.TextBox box)
        {
            int i = 0;
            List<char> toCheck = StringToCharList(box.Text);

            foreach (char c in box.Text)
            {
                Console.WriteLine(IsInt(c));
                if (!IsInt(c))
                {
                    toCheck.RemoveAt(i);
                    i--;
                }
                i++;
            }

            box.Text = CharListToString(toCheck);
        }

        public static List<byte> ByteArrayToList(byte[] g)
        {

            List<byte> j = new List<byte>();
            foreach (byte h in g)
            {
                j.Add(h);
            }
            return j;
        }

        public static byte[] ByteListToArray(List<byte> h)
        {

            byte[] k = new byte[h.Count];
            int i = 0;
            foreach (byte j in h)
            {
                k[i] = j;
                i++;
            }
            return k;
        }

        public static string CharListToString(List<char> c)
        {
            string d = "";
            foreach (char e in c)
            {
                d = d + e.ToString();
            }
            return d;
        }

        public static List<char> StringToCharList(string a)
        {

            List<char> parsed = new List<char>();
            foreach (char b in a)
            {
                parsed.Add(b);
            }

            return parsed;
        }

        public static bool IsInt(char a)
        {
            return int.TryParse(a.ToString(), out int dis);
        }
    }
}
