using System.Drawing;

namespace ImageWriter;

public class ConsolePresenter
{
    public static void Present(Color[,] traceResult)
    {
        for (var i = 0; i < traceResult.GetLength(0); i++)
        {
            for(int j = 0; j < traceResult.GetLength(1); j++)
            {
                var val = traceResult[i, j].ToArgb(); // ×È ÁÓÄÅ ÒÓÒ Â²Ä 0 ÄÎ 1 ÇÍÀ×ÅÍÍß???
                var filler = val switch
                {
                    var exp when val < 0 => "  ",
                    var exp when (val >= 0 && val < 0.2) => ". ",
                    var exp when (val >= 0.2 && val < 0.5) => "* ",
                    var exp when (val >= 0.5 && val < 0.8) => "o ",
                    _ => "# "
                };
                Console.Write(filler);
            }
            Console.WriteLine();
        }
    }
}