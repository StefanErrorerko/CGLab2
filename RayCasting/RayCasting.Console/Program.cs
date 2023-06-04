using RayCasting.Console;
using RayCasting.Core;
using RayCasting.Core.Objects;
using RayCasting.Core.ObjLoader;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

const string sourceFlag = "source";
const string outputFlag = "output";

var parser = new CommandLineParser(args);

var source = parser.GetArgument(sourceFlag);
var output = parser.GetArgument(outputFlag);

source = @"/Users/bohdankonopolskyi/Downloads/Telegram Desktop/cow.obj";
var fileData = File.ReadAllLines(source);
// Mesh mesh = ObjReader.ReadToMesh(fileData);
// mesh.Print();

var triangles = ObjReader.ReadToTriangles(fileData);

