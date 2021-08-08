using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace idx3_ubyte_parser
{
    internal class Program
    {
        /// <summary>
        ///
        /// </summary>

        private static string file_path;
        private static OpenFileDialog OFD = new OpenFileDialog();
        private static byte[] IDX3_File { get; set; }

        private static BinReader DataStream { get; set; }
        private static BinReader LabelStream { get; set; }

        private static IDX3 FileStruct { get; set; }

        [STAThread]
        private static void Main(string[] args)
        {
            Console.Title = "IDX3 Parser";
            OFD.Filter = "(*.idx3-ubyte)|*.idx3-ubyte";
            OFD.Title = "IDX3 Image File";
            OFD.ShowDialog();
            IDX3_File = File.ReadAllBytes(OFD.FileName);
            OFD.Title = "IDX1 Label File";
            OFD.Filter = "(*.idx1-ubyte)|*.idx1-ubyte";
            OFD.ShowDialog();
            LabelStream = new BinReader(new MemoryStream(File.ReadAllBytes(OFD.FileName)));
            DataStream = new BinReader(new MemoryStream(IDX3_File));
            parseFile();
            Console.Read();
        }

        private static void parseFile()
        {
            FileStruct = new IDX3();

            FileStruct.contentType = (IDX3.ContentType)Enum.Parse(typeof(IDX3.ContentType), DataStream.ReadInt32().ToString());
            FileStruct.ContentSize = DataStream.ReadInt32();

            LabelStream.ReadInt32();
            LabelStream.ReadInt32();

            //Only exists in Image Files
            if (FileStruct.contentType == IDX3.ContentType.image)
            {
                byte[][] pixies = new byte[28][];
                for (int i = 0; i < pixies.Length; ++i)
                {
                    pixies[i] = new byte[28];
                }

                FileStruct.Rows = DataStream.ReadInt32();
                FileStruct.Columns = DataStream.ReadInt32();

                Console.WriteLine("Found: " + FileStruct.ContentSize + " of type " + FileStruct.contentType.ToString() + " How many do you want extract?");
                int Max = int.Parse(Console.ReadLine());

                Console.WriteLine("Output Path?");
                string opt_path = Console.ReadLine();

                for (int di = 0; di < Max; ++di)
                {
                    for (int i = 0; i < 28; ++i)
                    {
                        for (int j = 0; j < 28; ++j)
                        {
                            byte b = DataStream.ReadByte();
                            pixies[i][j] = b;
                        }
                    }
                    byte _Label = LabelStream.ReadByte();
                    var image = TransformPixels(pixies);

                    Image img = new Image()
                    {
                        Label = _Label,
                        Pixels = pixies,
                        Bmp = image
                    };

                    FileStruct.Images.Add(img);
                }
                Console.WriteLine("Created: " + FileStruct.Images.Count.ToString() + " Images.");
                Extract(FileStruct, opt_path);
                Console.WriteLine("Extracted Files.");
            }
        }

        private static string GetImg(byte[][] pix)
        {
            int count = 0;
            string s = "";
            for (int i = 0; i < 28; ++i)
            {
                for (int j = 0; j < 28; ++j)
                {
                    if (pix[i][j] == 0)
                        s += " "; // white
                    else if (pix[i][j] == 255)
                        s += "%"; // black
                    else
                        s += ".";
                }
                s += "\n";
            }
            return s;
        }

        private static Bitmap TransformPixels(byte[][] pix)
        {
            Bitmap bmp = new Bitmap(28, 28);

            for (int i = 0; i < 28; ++i)
            {
                for (int j = 0; j < 28; ++j)
                {
                    if (pix[i][j] == 0)
                    {
                        bmp.SetPixel(i, j, Color.White);
                    } // white
                    else if (pix[i][j] == 255)
                    {
                        bmp.SetPixel(i, j, Color.Black);
                    } // black
                    else
                    {
                        bmp.SetPixel(i, j, Color.Orchid);
                    }
                }
            }

            return bmp;
        }

        private static void Extract(IDX3 _struct, string path)
        {
            for (int i = 0; i < _struct.Images.Count; i++)
            {
                if (!Directory.Exists(path + "\\" + _struct.Images[i].Label.ToString()))
                {
                    Directory.CreateDirectory(path + "\\" + _struct.Images[i].Label.ToString());
                }

                BitmapExtensions.SaveJPG100(_struct.Images[i].Bmp, path + "\\" + _struct.Images[i].Label.ToString() + $"\\{i}.jpg");
            }
        }
    }
}