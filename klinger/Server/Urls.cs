namespace klinger.Server
{
    using System;

    public class Urls
    {
        //http://machine-name:8008/
        public static Uri Root(string host, int port)
        {
            var ub = new UriBuilder("http", host, port);
            return ub.Uri;
        }

        //http://machine-name:8008/klinger
        public static Uri Dashboard()
        {
            var ub = new Uri(Root(Environment.MachineName, 8008), "klinger");
            return ub;
        }

        public static Uri Client(string name)
        {
            var ub = new Uri(Dashboard(), name);
            return ub;
        }
    }
}