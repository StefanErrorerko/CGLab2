using RayCasting.Core.Objects;
using RayCasting.Core.ObjLoader;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;
using System.Drawing;
using ImageWriter;
using PPMWriter;
using RayCasting.Console;
using RayCasting.Core.Misc;
using RayCasting.Core.Scene;

const string sourceFlag = "source";
const string outputFlag = "output";

var parser = new CommandLineParser(args);

var source = parser.GetArgument(sourceFlag);
var output = parser.GetArgument(outputFlag);

source = @"/Users/bohdankonopolskyi/Downloads/Telegram Desktop/cow.obj";
var fileData = File.ReadAllLines(source);

//Mesh mesh = ObjReader.ReadToMesh(fileData);
//mesh.print();

var transverter = new Transverter();
transverter.MoveX(-2);
var camera = new Camera(
    origin: new Point3(0, 0, 0), 
    direction: new Vector3(1, 0, 0), 
    distance: 1, 
    fieldOfView: 60,
    transverter);

 var triangles = ObjReader.ReadToTriangles(fileData);
var singleTriangle = new Triangle(new Point3(10, 0, 0), new Point3(3, 0.5f, 2.8f), new Point3(3, -2.5f, -0.5f));
// var triangles = new List<Triangle>
// {
//     new Triangle(new Point3(3, 0, 0), new Point3(3, 0.5f, 2.8f), new Point3(3, -2.5f, -0.5f))
// };

var objectTransverter = new Transverter();
// objectTransverter.RotateAngleZ(75);
// objectTransverter.RotateAngleX(-75);
// var transformedTriangles = new List<Triangle>();
// foreach(var triangle in triangles)
// {
//     transformedTriangles.Add(objectTransverter.ApplyTransformation(triangle));
// }

var scene = new BvhScene(light: new Point3(5.5f, 1.0f, 2f));
foreach (var triangle in triangles)
{
    scene.Objects.Add(triangle);
}
var disk = new Disk(new Vector3(0, 0, 1), new Vector3(0, 0, -0.5f), 50);
// var sphere = new Sphere(center: new Vector3(x: 0, y: 0, z: 4), radius: 100);        
// scene.Objects.Add(sphere);

var rayTracer = new BvhRayTracer(camera, scene);
var pixels = rayTracer.Trace();

var width = pixels.GetLength(1);
var height = pixels.GetLength(0);
var binaryData = new byte[width * height * 3];
//var colors = new Color[width, height];

//for (int y = 0; y < height; y++)
//{
//    for (int x = 0; x < width; x++)
//    {
//        byte greyscale = (byte)(pixels[y, x] * 255.0f);
//        const int channels = 3;
//        binaryData[y * width * channels + x * channels + 0] = greyscale;
//        binaryData[y * width * channels + x * channels + 1] = greyscale;
//        binaryData[y * width * channels + x * channels + 2] = greyscale;

//        colors[x, y] = Color.FromArgb(greyscale, greyscale, greyscale);
//    }
//}

var imageWriter = new PpmWriter();
File.WriteAllBytes(@"/Users/bohdankonopolskyi/Desktop/NewCowShadow.ppm", imageWriter.Write(pixels));
Console.WriteLine("Finish");

ConsolePresenter.Present(pixels);

//Console.ReadLine();



