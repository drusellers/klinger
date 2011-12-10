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
namespace klinger
{
    using Stact;
    using System;
    using Server;

    public static class KlingerConfigurator
    {
        static InProcessKlingerWebServer _webServer;
        static InProcessKlingerServer _server;
        static ActorInstance _validatorRepository;
        static ActorInstance _voteRepository;

        public static KlingerServer BuildAndStart(Action<KlingerConfiguration> action)
        {
            var healthFac = ActorFactory.Create(inbox => new InMemoryHealthVoteRepository(inbox));
            _voteRepository = healthFac.GetActor();

            var repoFac = ActorFactory.Create(inbox => new EnvironmentValidatorRepository(inbox, _voteRepository));
            _validatorRepository = repoFac.GetActor();

            var b = new BackingConfigurationObject(_validatorRepository);
            action(b);

            if (b.ShouldHostWebServerInProcess)
            {
                _webServer = new InProcessKlingerWebServer(b.Port, _validatorRepository);
                _webServer.Start();
            }

            if (b.ShouldRunScheduler)
            {
                _server = new InProcessKlingerServer(b.SchedulerDelay, b.SchedulerInterval, _validatorRepository);
                
                _server.OnFatal(b.ErrorAction);
                _server.OnWarning(b.WarningAction);
                _server.Start();
            }

            return _server;
        }
    }
}