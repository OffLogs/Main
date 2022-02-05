using CommandLine;

namespace OffLogs.Console.Verbs
{
    [Verb("email-send", HelpText = "Sends test email")]
    public class EmailSendVerb
    {
        [Option('e', "email", Required = true, HelpText = "Receiver's email")]
        public string Email { get; set; }
        
        public int Handle()
        {
            System.Console.WriteLine("User is sent to: " + Email);
            return 0;
        }
    }
}
