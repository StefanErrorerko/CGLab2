using ImageWriter;
using PPMWriter;
using RayCasting.Console;
using RayCasting.Core;
using RayCasting.Core.Misc;
using RayCasting.Core.Objects;
using RayCasting.Core.ObjLoader;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;
using System.Drawing;
using System.Numerics;

const string sourceFlag = "source";
const string outputFlag = "output";

var parser = new CommandLineParser(args);

var source = parser.GetArgument(sourceFlag);
var output = parser.GetArgument(outputFlag);

source = @"/Users/chere/Downloads/cow.obj";
var fileData = File.ReadAllLines(source);

//Mesh mesh = ObjReader.ReadToMesh(fileData);
//mesh.print();

var origin = new Point3(0, 0, 0);
var direction = new RayCasting.Core.Structures.Vector3(1, 0, 0);
var transverter = new Transverter();
transverter.MoveX(-2);
var camera = new Camera(origin, direction, distance: 1, fieldOfView: 30, transverter);
var triangles = ObjReader.ReadToTriangles(fileData);
var scene = new Scene(light: new Point3(-2.0f, 1.0f, 1.5f));
var objectTransverter = new Transverter();
objectTransverter.RotateAngleZ(90);
objectTransverter.RotateAngleX(-90);
var transformedTriangles = new List<Triangle>();
foreach(var triangle in triangles)
{
    transformedTriangles.Add(objectTransverter.ApplyTransformation(triangle));
}

foreach (var triangle in transformedTriangles)
{
    scene.Objects.Add(triangle);
}

var rayTracer = new RayTracer(camera, scene);
var pixels = rayTracer.Trace();

int width = pixels.GetLength(1);
int height = pixels.GetLength(0);
byte[] binary = new byte[width * height * 3];
var colors = new Color[width, height];

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        byte greyscale = (byte)(pixels[y, x] * 255.0f);
        const int channels = 3;
        binary[y * width * channels + x * channels + 0] = greyscale;
        binary[y * width * channels + x * channels + 1] = greyscale;
        binary[y * width * channels + x * channels + 2] = greyscale;
        colors[x, y] = Color.FromArgb(greyscale, greyscale, greyscale);
    }
}

var imageWriter = new PpmWriter();
File.WriteAllBytes(@"C:/Users/chere/Downloads/NewCowShadow.ppm", imageWriter.Write(colors));
Console.WriteLine("Finish");

ConsolePresenter.Present(pixels);



Console.ReadLine();






