using System;
using Akka.Actor;

namespace AkkaMjrOne.Step4.Completed
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
            if(message is StartTail)
            {
                var msg = message as StartTail;

                var props = _forMono
                    ? Props.Create(() => new Mono.TailActor(msg.ReporterActor, msg.FilePath))
                    : Props.Create(() => new TailActor(msg.ReporterActor, msg.FilePath));

                Context.ActorOf(props);
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            // create a OneForOneStrategy that has the following traits:
            // - maximum of 10 retries per 30 seconds
            // - Resume on ArithmeticException
            // - Stop on NotSupportedException
            // - Restart on all other exceptions
            return new OneForOneStrategy(
                10, // maxNumberOfRetries
                TimeSpan.FromSeconds(30), // duration
                x =>
                {
                    //Maybe we consider ArithmeticException to not be application critical
                    //so we just ignore the error and keep going.
                    if (x is ArithmeticException) return Directive.Resume;

                    //Error that we cannot recover from, stop the failing actor
                    else if (x is NotSupportedException) return Directive.Stop;

                    //In all other cases, just restart the failing actor
                    else return Directive.Restart;
                });
        }
    }
}
