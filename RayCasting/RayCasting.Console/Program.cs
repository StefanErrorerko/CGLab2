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

const string sourceFlag = "source";
const string outputFlag = "output";

var parser = new CommandLineParser(args);

var source = parser.GetArgument(sourceFlag);
var output = parser.GetArgument(outputFlag);

source = @"/Users/chere/Downloads/cow.obj";
var fileData = File.ReadAllLines(source);

//Mesh mesh = ObjReader.ReadToMesh(fileData);
//mesh.print();

var transverter = new Transverter();
transverter.MoveX(-2);
var camera = new Camera(
    origin: new Point3(9, 0, 0), 
    direction: new Vector3(1, 0, 0), 
    distance: 1, 
    fieldOfView: 60,
    transverter);

var triangles = ObjReader.ReadToTriangles(fileData);

var objectTransverter = new Transverter();
objectTransverter.RotateAngleZ(75);
objectTransverter.RotateAngleY(180);
objectTransverter.RotateAngleX(75);
objectTransverter.MoveX(8);

var transformedTriangles = new List<Triangle>();
foreach (var triangle in triangles)
{
    transformedTriangles.Add(objectTransverter.ApplyTransformation(triangle));
}

var scene = new Scene(light: new Point3(5.5f, 1f, 2f));
foreach (var triangle in transformedTriangles)
{
    scene.Objects.Add(triangle);
}
scene.Objects.Add(new Sphere(new Vector3(9.2f, 0.6f, 0.7f), 0.25f));
scene.Objects.Add(new Disk(new Vector3(9.2f, 0.6f, 0.5f), new Vector3(-1, 0, 0), 0.4f));
scene.Objects.Add(new Triangle(new Point3(9.0f, 1, -1), new Point3(9.1f, 0.7f, -0.7f), new Point3(9.4f, 0.6f, -0.7f)));

var rayTracer = new RayTracer(camera, scene);
var pixels = rayTracer.Trace();
var colors = TraceResultConverter.ConvertToGrayscalePixels(pixels);

ConsolePresenter.Present(pixels);

var imageWriter = new PpmWriter();
File.WriteAllBytes(@"C:/Users/chere/Downloads/NewCowShadow.ppm", imageWriter.Write(colors));

Console.ReadLine();






