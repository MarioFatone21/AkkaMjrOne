using System;
using Akka.Actor;

namespace AkkaMjrOne.Step6.Completed
{
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // make actor system 
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            // create top-level actors within the actor system
            var consoleWriterProps = Props.Create<ConsoleWriterActor>();
            var consoleWriter = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            // Set the "forMono" flag to true if you are using VS for Mac
            var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor(true));
            var tailCoordinator = MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

            var fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriter));
            var fileValidator = MyActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

            var consoleReaderProps = Props.Create<ConsoleReaderActor>(fileValidator);
            var consoleReader = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // begin processing
            consoleReader.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
}
