using CommandLine;

namespace OffLogs.Console.Verbs
{
    [Verb("user-create", HelpText = "Creates new user with application")]
    public class CreateNewUserVerb
    {
        [Option('u', "username", Required = true, HelpText = "Name of new user.")]
        public string UserName { get; set; }
        
        [Option('e', "email", Required = true, HelpText = "Email of new user.")]
        public string Email { get; set; }
        
        public int Handle()
        {
            System.Console.WriteLine("User is created: " + UserName);
            return 0;
        }
    }
}