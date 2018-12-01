using System;
using Akka.Actor;

namespace AkkaMjrOne.Step2
{
    public class ConsoleReaderActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public const string StartCommand = "start";
        public const string ExitCommand = "exit";
        public const string ContinueCommand = "continue";

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }
            else if (message is Messages.InputError)
            {
                // send message to consoleWriterActor
                // TODO
            }

            GetAndValidateInput();
        }


        #region Internal methods

        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }

        /// <summary>
        /// Reads input from console, validates it, then signals appropriate response
        /// (continue processing, error, success, etc.).
        /// </summary>
        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                // signal that the user needs to supply an input, as previously 
                // received input was blank
                // TODO
            }
            else if (string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // shut down the entire actor system (allows the process to exit)
                // TODO

                return;
            }
            else
            {
                var valid = IsValid(message);
                if (valid)
                {
                    // send message to consoleWriterActor
                    // TODO

                    // continue reading messages from console
                    // TODO
                }
                else
                {
                    // send validation error
                    // TODO
                }
            }
        }

        /// <summary>
        /// Validates <see cref="Messages"/>.
        /// Currently says messages are valid if contain even number of characters.
        /// </summary>
        /// <param name="message"></param>
        private static bool IsValid(string message)
        {
            var valid = message.Length % 2 == 0;
            return valid;
        }

        #endregion
    }
}
