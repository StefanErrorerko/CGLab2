using RayCasting.Console;
using RayCasting.Core.Objects;
using RayCasting.Core.ObjLoader;

const string sourceFlag = "source";
const string outputFlag = "output";

var parser = new CommandLineParser(args);

var source = parser.GetArgument(sourceFlag);
var output = parser.GetArgument(outputFlag);

Mesh mesh = ObjLoader.LoadObj(@"C:\Users\kadde\Downloads\cow.obj");
mesh.Print();