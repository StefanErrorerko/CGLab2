// See https://aka.ms/new-console-template for more information

using ImageConverter.Console;
using ImageConverter.Orchestrator;

const string sourceFlag = "source";
const string objectiveExtensionFlag = "goal-format";
const string outputFlag = "output";

var parser = new CommandLineParser(args);

var objectiveExtension = parser.GetArgument(objectiveExtensionFlag);
var source = parser.GetArgument(sourceFlag);
var output = parser.GetArgument(outputFlag);

ConverterOrchestrator converter = new ConverterOrchestrator();
converter.Convert(source, objectiveExtension);
 Console.ReadKey();