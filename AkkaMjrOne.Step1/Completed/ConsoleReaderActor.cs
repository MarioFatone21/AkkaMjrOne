using System;
using Akka.Actor;

namespace AkkaMjrOne.Step1.Completed
{
    // Make an actor of this class
    public class ConsoleReaderActor//TODO
        : UntypedActor
    {
        private IActorRef _consoleWriterActor;

        public const string ExitCommand = "exit";
        public const string ContinueCommand = "continue";

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        //TODO
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
                // TODO
                Context.System.Terminate();

                return;
            }

            // send input to the console writer to process and print
            // TODO
            _consoleWriterActor.Tell(read);

            // continue reading messages from the console
            // TODO
            Self.Tell(ContinueCommand);
        }
    }
}
