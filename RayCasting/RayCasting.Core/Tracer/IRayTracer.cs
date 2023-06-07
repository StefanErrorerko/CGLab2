using System.Drawing;

namespace RayCasting.Core.Tracer;

public interface IRayTracer
{
    Color[,] Trace();
}