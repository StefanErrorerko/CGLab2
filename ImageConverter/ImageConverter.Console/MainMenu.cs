namespace ImageConverter.Console;

public class MainMenu
{
    private string[] _args;

    public MainMenu(string[] args)
    {
        _args = args;
    }

    //first menu
    public string Display()
    {
        // Console.WriteLine("Hello, stranger!");
        if (_args.Length != 2) MenuIfNoArguments();
        return _args[0];
    }

    //menu if no arguments given with start of the program
    public string MenuIfNoArguments()
    {
        // Console.WriteLine("Please type an image source path and output image format using space:");
        // while (true)
        //     try
        //     {
        //         _args = (Console.ReadLine() ?? throw new NullReferenceException()).Split(' ');
        //         break;
        //     }
        //     catch (NullReferenceException)
        //     {
        //         Console.WriteLine("You input nothing.");
        //     }
        //
        // return _args[0];

        throw new NotImplementedException();
    }

    public string[] MenuFormatCheck(string[] formats)
    {
        string inputFormat;
        var index = _args[0].IndexOf('.');
        if (index >= 0)
        {
            inputFormat = _args[0][(index + 1)..];
            if (IfSupportedFormat(formats, inputFormat) && IfSupportedFormat(formats, _args[1]))
                return new[] { inputFormat, _args[1] };
        }

        MenuIfNoArguments();
        var result = MenuFormatCheck(formats);
        return result;
    }

    public void LastMenu()
    {
       // Console.WriteLine("Successfully converted.");
    }

    public bool IfSupportedFormat(string[] formats, string format)
    {
        if (formats.Contains(format)) return true;
        return false;
    }
}