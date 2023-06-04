namespace ImageWriter;

public class ConsolePresenter
{
    public static void Present(float[,] traceResult)
    {
        for (var i = 0; i < traceResult.GetLength(0); i++)
        {
            for (var j = 0; j < traceResult.GetLength(1); j++)
            {
                var c = ' ';
                if (traceResult[i, j] > float.Epsilon) c = '#';
                Console.Write(c);
            }

            Console.WriteLine();
        }
    }
}