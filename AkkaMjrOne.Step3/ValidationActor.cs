using Akka.Actor;

namespace AkkaMjrOne.Step3
{
    public class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        // put validation logic here
        // TODO
        protected override void OnReceive(object message)
        {

        }
    }
}
