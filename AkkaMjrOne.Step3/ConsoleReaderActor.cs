using System;
using Akka.Actor;

namespace AkkaMjrOne.Step3
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
            // get the message from the console, check if it isn't the exit command, if not, 
            // validate using the new validationActor
            // TODO
        }

        #endregion
    }
}
