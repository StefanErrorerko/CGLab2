using RayCasting.Console;
using RayCasting.Core.Objects;
using RayCasting.Core.ObjLoader;

const string sourceFlag = "source";
const string outputFlag = "output";

var parser = new CommandLineParser(args);

var source = parser.GetArgument(sourceFlag);
var output = parser.GetArgument(outputFlag);

source = @"C:\Users\kadde\Downloads\cow.obj";
var fileData = File.ReadAllLines(source);
Mesh mesh = ObjReader.ReadToMesh(fileData);
mesh.Print();