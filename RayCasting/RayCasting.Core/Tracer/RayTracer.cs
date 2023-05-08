using RayCasting.Core.Objects;

namespace RayCasting.Core.Tracer;

public class RayTracer : IRayTracerProtocol
{
    //MARK: - Initialization

    public RayTracer(int width, int height, ICameraProtocol camera, Scene scene)
    {
        ImageWidth = width;
        ImageHeight = height;
        Camera = camera;
        Scene = scene;
    }

    public RayTracer(int width, (float width, float height) aspectRatio, ICameraProtocol camera, Scene scene)
    {
        var height = (int)(width / aspectRatio.width * aspectRatio.height);
        ImageWidth = width;
        ImageHeight = height;
        Camera = camera;
        Scene = scene;
    }
    //MARK: - Properties

    public int ImageWidth { get; }
    public int ImageHeight { get; }
    public ICameraProtocol Camera { get; }
    public Scene Scene { get; }

    //MARK: - Public methods

    public float[][] Trace()
    {
        var image = new float[ImageHeight][];
        for (var i = 0; i < ImageHeight; i++)
        {
            image[i] = new float[ImageWidth];
            for (var j = ImageWidth - 1; j >= 0; j--)
            {
                var u = (float)j / (ImageWidth - 1);
                var v = (float)i / (ImageHeight - 1);
                var castRay = Camera.GetRay(u, v);

                var closest = GetClosestObject(Scene.Objects, castRay);

                if (closest.T >= 0)
                {
                    var point = castRay.PointAt(closest.T);

                    image[i][j] = closest.Object.Reflects(Scene.Light, point);
                }
            }
        }

        return image;
    }

    //MARK: - Private methods

    private (IObjectProtocol Object, float T) GetClosestObject(IList<IObjectProtocol> objects, Ray ray)
    {
        IObjectProtocol closestObject = null;
        float? closestT = null;

        foreach (var obj in objects)
        {
            var t = obj.Intersects(ray);
            if (t.HasValue && (closestT == null || t < closestT))
            {
                closestObject = obj;
                closestT = t;
            }
        }

        return (Object: closestObject, T: closestT ?? -1);
    }
}