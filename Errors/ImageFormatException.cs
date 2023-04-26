namespace CGLab2.Errors;

public class ImageFormatException : Exception
{
    public class ImageFormatException : FormatException
    {
        public ImageFormatException() 
            : base() 
        { }
        public ImageFormatException(string message) 
            : base(message)
        {

        }
    }
}