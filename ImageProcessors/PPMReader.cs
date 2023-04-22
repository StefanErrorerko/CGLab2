using CGLab2.Images;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGLab2.ImageProcessors
{
    public class PPMReader : IImageReader
    {
        String filePath;
        public PPMReader(String path) 
        {
            filePath = path;
        }

        //public ImagePPM ReadP6()
        //{
        //    using (FileStream fs = new FileStream(filePath, FileMode.Open))
        //    {
        //        using (BinaryReader reader = new BinaryReader(fs))
        //        {
        //            // Read the PPM header
        //            string header = new string(reader.ReadChars(2));
        //            if (header != "P6")
        //            {
        //                Console.WriteLine("Error: Invalid PPM file format.");
        //                return null;
        //            }

        //            // Read the image dimensions
        //            string dimensions = "";
        //            char c = ' ';
        //            c = (char)reader.ReadByte();
        //            do
        //            {
        //                c = (char)reader.ReadByte();
        //                dimensions += c;
        //            } while (c != '\n');
        //            dimensions = dimensions.Trim();
        //            string[] dimensionValues = dimensions.Split(' ');
        //            int width = int.Parse(dimensionValues[0]);
        //            int height = int.Parse(dimensionValues[1]);

        //            // Read the maximum color value
        //            string maxColorValue = "";
        //            c = ' ';
        //            while (c != '\n')
        //            {
        //                c = (char)reader.ReadByte();
        //                maxColorValue += c;
        //            }
        //            maxColorValue = maxColorValue.Trim();
        //            int maxVal = int.Parse(maxColorValue);

        //            // Read the pixel data
        //            byte[] pixelData = reader.ReadBytes(width * height * 3);
        //            Color[,] pixels = new Color[width*3, height];

        //            // Print the pixel data
        //            for (int i = 0; i < height; i++)
        //            {
        //                for (int j = 0; j < width * 3; j += 3)
        //                {
        //                    int r = pixelData[i * width * 3 + j];
        //                    int g = pixelData[i * width * 3 + j + 1];
        //                    int b = pixelData[i * width * 3 + j + 2];
        //                    pixels[i, j] = Color.FromArgb(r, g, b);
        //                }
                    
        //            }
        //            return new ImagePPM(header, width, height, maxVal, pixels);
        //        }
        //    }
        //}

        public ImagePPM ReadP3()
        {
            int width = 0, 
                height = 0, 
                maxColor = 0;
            Color[,] pixels = new Color[width, height];

            // read the PPM file header
            using (StreamReader reader = new StreamReader(filePath))
            {
                if (reader.ReadLine() != "P3")
                {
                    Console.WriteLine("Error: Invalid magic number");
                    return null;
                }

                // read image dimensions
                string[] dimensions = reader.ReadLine().Split(' ');
                width = int.Parse(dimensions[0]);
                height = int.Parse(dimensions[1]);
                pixels = new Color[width, height];

                // read max color value
                maxColor = int.Parse(reader.ReadLine());

                // read the PPM file pixel data
                // read pixel data
                for (int j = 0; j < height; j++)
                {
                    string line = reader.ReadLine();
                    if (line.StartsWith("#"))
                    {
                        line = reader.ReadLine();
                        continue;
                    }
                    string[] pixelValues = line.Split(' ');
                    for (int i = 0; i < width; i += 3)
                    {
                        int r = Int32.Parse(pixelValues[i]);
                        int g = Int32.Parse(pixelValues[i + 1]);
                        int b = Int32.Parse(pixelValues[i + 2]);
                        pixels[i, j] = Color.FromArgb(r, g, b);
                    }
                }
            }
            var ppm = new ImagePPM(width, height, pixels);
            return ppm;
        }
    }
}
