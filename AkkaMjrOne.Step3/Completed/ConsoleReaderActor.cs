using System;
using Akka.Actor;

namespace AkkaMjrOne.Step3.Completed
{
    public class ConsoleReaderActor : UntypedActor
    {
        private readonly IActorRef _validationActor;

        public const string StartCommand = "start";
        public const string ExitCommand = "exit";
        public const string ContinueCommand = "continue";

        public ConsoleReaderActor(IActorRef validationActor)
        {
            _validationActor = validationActor;
        }

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
            if (!string.IsNullOrEmpty(message) && String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // if user typed ExitCommand, shut down the entire actor system (allows the process to exit)
                Context.System.Terminate();
                return;
            }

            // otherwise, just hand message off to validation actor (by telling its actor ref)
            _validationActor.Tell(message);
        }

        #endregion
    }
}
