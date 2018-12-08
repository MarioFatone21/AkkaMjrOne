using System;
using Akka.Actor;

namespace AkkaMjrOne.Step6
{
    public class ConsoleReaderActor : UntypedActor
    {
        public const string StartCommand = "start";
        public const string ExitCommand = "exit";
        public const string ContinueCommand = "continue";

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }

            GetAndValidateInput();
        }


        #region Internal methods

        private void DoPrintInstructions()
        {
            Console.WriteLine("Please provide the URI of a log file on disk.\n");
        }

        /// <summary>
        /// Reads input from console, validates it, then signals appropriate response
        /// (continue processing, error, success, etc.).
        /// </summary>
        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (!string.IsNullOrEmpty(message) && String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // if user typed ExitCommand, shut down the entire actor system (allows the process to exit)
                Context.System.Terminate();
                return;
            }

            // otherwise, just hand message off to validation actor (by telling its actor ref)
            Context.ActorSelection("akk://MyActorSystem/user/validationActor");
        }

        #endregion
    }
}
