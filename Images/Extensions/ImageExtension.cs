namespace CGLab2.Images.Extensions
{
    public static class ImageExtension
    {
        public static ImageBMP ToBMP(this Image image)
        {
            var bmp = new ImageBMP(image.Width, image.Height, image.Pixels);
            return bmp;
        }
        public static ImagePPM ToPPM(this Image image) 
        {
            var ppm = new ImagePPM(image.Width, image.Height, image.Pixels);
            return ppm;
        }
    }
}
