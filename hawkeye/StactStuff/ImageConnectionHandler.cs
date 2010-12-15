namespace hawkeye.Client
{
    using System.Linq;
    using Magnum.Extensions;
    using Stact;
    using Stact.Internal;
    using Stact.ServerFramework;

    public class ImageConnectionHandler :
        PatternMatchConnectionHandler
    {
        readonly ImageChannel _statusChannel;

        public ImageConnectionHandler() :
            base(".png$", "GET")
        {
            _statusChannel = new ImageChannel();
        }

        protected override Channel<ConnectionContext> CreateChannel(ConnectionContext context)
        {
            return _statusChannel;
        }


        class ImageChannel :
            Channel<ConnectionContext>
        {
            readonly Fiber _fiber;

            public ImageChannel()
            {
                _fiber = new PoolFiber();
            }

            public void Send(ConnectionContext context)
            {
                _fiber.Add(() =>
                {
                    var localPath = context.Request.Url.LocalPath;
                    var imageName = localPath.Split('/').Last();
                    context.Response.ContentType = "image/png";
                    using(var str = GetType().Assembly.GetManifestResourceStream("hawkeye.images."+imageName))
                    {
                        var buff = str.ReadToEnd();
                        context.Response.OutputStream.Write(buff, 0, buff.Length);
                    }
                    context.Complete();
                });
            }
        }
    }
    
    public class CssConnectionHandler :
        PatternMatchConnectionHandler
    {
        readonly CssChannel _statusChannel;

        public CssConnectionHandler() :
            base(".css$", "GET")
        {
            _statusChannel = new CssChannel();
        }

        protected override Channel<ConnectionContext> CreateChannel(ConnectionContext context)
        {
            return _statusChannel;
        }


        class CssChannel :
            Channel<ConnectionContext>
        {
            readonly Fiber _fiber;

            public CssChannel()
            {
                _fiber = new PoolFiber();
            }

            public void Send(ConnectionContext context)
            {
                _fiber.Add(() =>
                {
                    var localPath = context.Request.Url.LocalPath;
                    var cssName = localPath.Split('/').Last();
                    context.Response.ContentType = "text/css";
                    using(var str = GetType().Assembly.GetManifestResourceStream("hawkeye.styles."+cssName))
                    {
                        var buff = str.ReadToEnd();
                        context.Response.OutputStream.Write(buff, 0, buff.Length);
                    }
                    context.Complete();
                });
            }
        }
    }
}