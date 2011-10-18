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

using Stact;

namespace klinger
{
    using System;
    using Server;

    public static class KlingerConfigurator
    {
        static InProcessKlingerWebServer _webServer;
        static InProcessKlingerServer _server;
        static ActorInstance _repository;

        public static KlingerServer BuildAndStart(Action<KlingerConfiguration> action)
        {
            var repoFac = ActorFactory.Create(inbox => new EnvironmentValidatorRepository(inbox));
            _repository = repoFac.GetActor();

            var b = new BackingConfigurationObject(_repository);
            action(b);

            if (b.ShouldHostWebServerInProcess)
            {
                _webServer = new InProcessKlingerWebServer(b.Port, _repository);
                _webServer.Start();
            }

            if (b.ShouldRunScheduler)
            {
                _server = new InProcessKlingerServer(b.SchedulerDelay, b.SchedulerInterval);
                
                _server.OnFatal(b.ErrorAction);
                _server.OnWarning(b.WarningAction);
                _server.Start();
            }

            return _server;
        }
    }
}