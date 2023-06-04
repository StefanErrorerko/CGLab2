namespace ImageWriter;

public class ConsolePresenter
{
    public static void Present(float[,] traceResult)
    {
        for (var i = 0; i < traceResult.GetLength(0); i++)
        {
            for(int j = 0; j < traceResult.GetLength(1); j++)
            {
                var val = traceResult[i, j];
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