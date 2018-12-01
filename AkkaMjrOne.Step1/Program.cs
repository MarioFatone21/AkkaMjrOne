using System;
using Akka.Actor;

namespace AkkaMjrOne.Step1
{
    class Program
    {
        // ActorSystem is a heavy object: create only one per application
        private static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            // TODO

            PrintInstructions();

            // time to make your first actors! 
            // NOTE: use the variant that allows you to pass constructor params for ConsoleReaderActor
            // TODO

            // tell console reader to begin
            // TODO

            // blocks the main thread from exiting until the actor system is shut down
            // TODO

        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.Write("Some lines will appear as");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" red ");
            Console.ResetColor();
            Console.Write(" and others will appear as");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" green! ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }
    }
}
