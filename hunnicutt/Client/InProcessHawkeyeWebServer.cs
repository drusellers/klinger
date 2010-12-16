// Copyright 2007-2010 The Apache Software Foundation.
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace hawkeye.Client
{
    using System;
    using Stact.Internal;
    using Stact.ServerFramework;
    using Stact;
    using StactStuff;

    public class InProcessHawkeyeWebServer
    {
        static ChannelAdapter _input;
        static HttpServer _server;
        readonly int _port;
        HealthRepository _repo;

        public InProcessHawkeyeWebServer(int port, HealthRepository repo)
        {
            _port = port;
            _repo = repo;
        }

        public static Uri ServerUri { get; private set; }

        public void Start()
        {
            _input = new ChannelAdapter();


            ServerUri = new UriBuilder("http","localhost",_port, "hawkeye").Uri;
            _server = new HttpServer(ServerUri, new PoolFiber(), _input, new PatternMatchConnectionHandler[]
                                                                                   {
                                                                                       new VersionConnectionHandler(),
                                                                                       new StatusConnectionHandler(_repo),
                                                                                       new ImageConnectionHandler(),
                                                                                       new CssConnectionHandler()
                                                                                   });
            _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
        }
    }
}