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
namespace hawkeye.Config
{
    using System;
    using Client;

    public static class HawkeyeConfigurator
    {
        static InProcessHawkeyeServer _server;
        static HealthRepository _repository;

        public static void Configure(Action<HawkeyeConfiguration> action)
        {
            _repository = new HealthRepository();

            var b = new BackingConfigurationObject(_repository);
            action(b);

            if (b.ShouldHostInProcess)
            {
                _server = new InProcessHawkeyeServer(b.Port, _repository);
                _server.Start();
            }

            //how to stop?
        }
    }
}