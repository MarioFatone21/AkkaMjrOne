using Akka.Actor;

namespace AkkaMjrOne.Step2
{
    class Program
    {
        // ActorSystem is a heavy object: create only one per application
        private static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MajorOne");

            // time to make your first actors! 
            // NOTE: use the variant that allows you to pass constructor params for ConsoleReaderActor
            var consoleWriter = MyActorSystem.ActorOf<ConsoleWriterActor>("consoleWriterActor");
            var consoleReader = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriter)), "consoleReaderActor");

            // tell console reader to begin
            // TODO

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();

        }
    }
}
