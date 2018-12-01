using Akka.Actor;

namespace AkkaMjrOne.Step3.Completed
{
    class Program
    {
        // ActorSystem is a heavy object: create only one per application
        private static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MajorOne");


            // this is here to show you what NOT to do
            // this approach to props has no type safety
            // it will compile, but can easily blow up in your face at runtime :(
            // UNCOMMENT THE BELOW TWO LINES, BUILD THE SOLUTION, AND THEN TRY TO RUN IT TO SEE
            //Props fakeActorProps = Props.Create(typeof(FakeActor));
            //IActorRef fakeActor = MyActorSystem.ActorOf(fakeActorProps, "fakeActor");


            // set up actors, using props (split props onto own line so easier to read)
            var consoleWriterProps = Props.Create<ConsoleWriterActor>();
            var consoleWriter = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            var validationActorProps = Props.Create(() => new ValidationActor(consoleWriter));
            var validation = MyActorSystem.ActorOf(validationActorProps, "validationActor");

            var consoleReaderProps = Props.Create<ConsoleReaderActor>(validation);
            var consoleReader = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // tell console reader to begin
            consoleReader.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();

        }

        /// <summary>
        /// Fake actor / marker class. Does nothing at all, and not even an actor actually. 
        /// Here to show why you shouldn't use typeof approach to Props.
        /// </summary>
        public class FakeActor { }
    }
}
