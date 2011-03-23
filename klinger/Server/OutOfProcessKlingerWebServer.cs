namespace klinger.Server
{
    using Stact;
    using Stact.Internal;

    public class OutOfProcessKlingerWebServer
    {
        public UntypedChannel EventChannel;
        Fiber _fiber;
        

        public OutOfProcessKlingerWebServer()
        {
            _fiber = new PoolFiber();
            EventChannel = new ChannelAdapter();
        }



        //listens on an incoming named pipe
        //stores reported event data in the repo
        //done
    }
}