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
namespace klinger.Server.Http
{
    using Stact.ServerFramework;
    using Stact;
    using StactStuff;

    public class StatusConnectionHandler :
        PatternMatchConnectionHandler
    {
        readonly StatusChannel _statusChannel;

        public StatusConnectionHandler(ActorInstance repo) :
            base("^/status", "GET")
        {
            _statusChannel = new StatusChannel(repo);
        }

        protected override Channel<ConnectionContext> CreateChannel(ConnectionContext context)
        {
            return _statusChannel;
        }


        class StatusChannel :
            Channel<ConnectionContext>
        {
            readonly Fiber _fiber;
            readonly ActorInstance _repo;

            public StatusChannel(ActorInstance repo)
            {
                _repo = repo;
                _fiber = new PoolFiber();
            }

            public void Send(ConnectionContext context)
            {
                _fiber.Add(() =>
                {
                    context.Response.RenderSparkView(_repo, "status.html");
                    context.Complete();
                });
            }
        }
    }
}