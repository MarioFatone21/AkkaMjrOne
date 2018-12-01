using System;
using Akka.Actor;

namespace AkkaMjrOne.Step2
{
    // Make an actor of this class
    public class ConsoleReaderActor : UntypedActor
    {
        private IActorRef _consoleWriterActor;

        public const string ExitCommand = "exit";
        public const string ContinueCommand = "continue";

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            Process(message);
        }


        private void Process(object message)
        {
            var read = Console.ReadLine();
            if (!string.IsNullOrEmpty(read) && string.Equals(read, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                //shut down the system (aquire handle to system via this actors context)
                Context.System.Terminate();

                return;
            }

            // send input to the console writer to process and print
            _consoleWriterActor.Tell(read);

            // continue reading messages from the console
            Self.Tell(ContinueCommand);
        }
    }
}
