using CommandLine;

namespace OffLogs.Console.Verbs
{
    [Verb("application-create", HelpText = "Creates new application")]
    public class CreateNewApplicationVerb
    {
        [Option('u', "username", Required = true, HelpText = "Name of new user.")]
        public string UserName { get; set; }
        
        [Option('n', "name", Required = true, HelpText = "Name of application")]
        public string Name { get; set; }
    }
}