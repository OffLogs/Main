using CommandLine;

namespace OffLogs.Console.Verbs
{
    [Verb("user-create", HelpText = "Creates new user with application")]
    public class CreateNewUserVerb
    {
        [Option('u', "username", Required = true, HelpText = "Name of new user.")]
        public string UserName { get; set; }
        
        public int Handle()
        {
            System.Console.WriteLine("User is created: " + UserName);
            return 0;
        }
    }
}