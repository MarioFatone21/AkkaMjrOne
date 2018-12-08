using System;
using Akka.Actor;

namespace AkkaMjrOne.Step4
{
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // make actor system 
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            // create top-level actors within the actor system
            // Set the "forMono" flag to true for class TailCoordinatorActor if you are using VS for Mac
            // TODO

            // begin processing
            // TODO

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
}
