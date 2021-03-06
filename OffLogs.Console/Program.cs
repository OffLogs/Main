using System;
using System.Collections.Generic;
using CommandLine;
using OffLogs.Console.Verbs;

namespace OffLogs.Console
{
    class Program
    {
        static int Main(string[] args) {
            return CommandLine.Parser.Default.ParseArguments<CreateNewUserVerb>(args)
                .MapResult(
                    (CreateNewUserVerb opts) =>  opts.Handle(),
                    errs => 1);
        }
    }
}