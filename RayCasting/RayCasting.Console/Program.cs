using RayCasting.Console;
using RayCasting.Core;
using RayCasting.Core.Objects;
using RayCasting.Core.ObjLoader;
using RayCasting.Core.Tracer;
using RayCasting.Core.Structures;

const string sourceFlag = "source";
const string outputFlag = "output";

var parser = new CommandLineParser(args);

var source = parser.GetArgument(sourceFlag);
var output = parser.GetArgument(outputFlag);

source = @"C:\Users\chere\Downloads\cow.obj";
var fileData = File.ReadAllLines(source);
Mesh mesh = ObjReader.ReadToMesh(fileData);

var camera = new Camera(
    origin: new Vector3(0, 0, 0), 
    direction: new Vector3(0, 0, 1), 
    top: new Vector3(0, 1, 0), 
    fieldOfView: 60, 
    aspectRatio: (16, 9));
var scene = new Scene(
    new List<IObject> { mesh}, 
    new Vector3(
        start: new Point3(x: -1, y: -1, z: 0), 
        end:new Point3(x: 0, y: 0, z: 1)));
var rayTracer = new RayTracer(
    width: 40,
    aspectRatio: (16, 9),
    camera,
    scene);
var img = rayTracer.Trace();

mesh.Print();
Console.ReadKey();