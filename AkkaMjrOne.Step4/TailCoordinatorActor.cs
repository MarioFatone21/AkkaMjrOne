using System;
using Akka.Actor;

namespace AkkaMjrOne.Step4
{
    public class TailCoordinatorActor : UntypedActor
    {
        #region Message types

        /// <summary>
        /// Start tailing the file at user-specified path.
        /// </summary>
        public class StartTail
        {
            public StartTail(string filePath, IActorRef reporterActor)
            {
                FilePath = filePath;
                ReporterActor = reporterActor;
            }

            public string FilePath { get; private set; }

            public IActorRef ReporterActor { get; private set; }
        }

        /// <summary>
        /// Stop tailing the file at user-specified path.
        /// </summary>
        public class StopTail
        {
            public StopTail(string filePath)
            {
                FilePath = filePath;
            }

            public string FilePath { get; private set; }
        }

        #endregion

        private readonly bool _forMono;

        public TailCoordinatorActor(bool forMono = false)
        {
            _forMono = forMono;
        }

        protected override void OnReceive(object message)
        {
            // process message and create child actor (take in account the forMono flag)
            // TODO
            if(message is StartTail)
            {

            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            // create a OneForOneStrategy that has the following traits:
            // - maximum of 10 retries per 30 seconds
            // - Resume on ArithmeticException
            // - Stop on NotSupportedException
            // - Restart on all other exceptions
            // TODO
            throw new NotImplementedException();
        }
    }
}
