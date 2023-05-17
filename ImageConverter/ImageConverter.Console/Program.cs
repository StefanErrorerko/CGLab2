using ImageConverter.Console;
using ImageConverter.Orchestrator;

const string sourceFlag = "source";
const string objectiveExtentionFlag = "goal-format";
const string outputFlag = "output";

var parser = new CommandLineParser(args);

var objectiveExtention = parser.GetArgument(objectiveExtentionFlag);
var source = parser.GetArgument(sourceFlag);
var output = parser.GetOptionalArgument(outputFlag, source, objectiveExtention);

try
{
    FileConverter converter = new FileConverter();
    converter.Convert(source, objectiveExtention, output);
}
catch (Exception e)
{
    Console.WriteLine(e);
}