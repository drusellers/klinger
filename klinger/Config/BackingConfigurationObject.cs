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
    using System;
    using Magnum.Extensions;
    using Magnum.TypeScanning;
    using Server;
    using Server.Messages;

    public class BackingConfigurationObject :
        KlingerConfiguration
    {
        EnvironmentValidatorRepository _repository;

        public BackingConfigurationObject(EnvironmentValidatorRepository repository)
        {
            _repository = repository;
        }

        public void RegisterAllValidatorsInAssembly<T>()
        {
            var types = TypeScanner.Scan(cfg =>
            {
                cfg.AddAllTypesOf<EnvironmentValidator>();
                cfg.AssemblyContainingType<T>();
            });
            types.Each(_repository.AddCheck);
        }

        public void RegisterValidator<TValidator>() where TValidator : EnvironmentValidator, new()
        {
            _repository.AddCheck<TValidator>();
        }

        public void RegisterValidator(EnvironmentValidator validator)
        {
            _repository.AddCheck(validator);
        }

        public void RegisterValidator(Type t)
        {
            _repository.AddCheck(t);
        }

        public void HostWebServerInProcess(int port)
        {
            ShouldHostWebServerInProcess = true;
            Port = port;
        }

        public void ScheduleValidations(TimeSpan interval)
        {
            ScheduleValidations(5.Seconds(), interval);
        }

        public void ScheduleValidations(TimeSpan delayInterval, TimeSpan interval)
        {
            ShouldRunScheduler = true;
            SchedulerInterval = interval;
            SchedulerDelay = delayInterval;
        }

        public void OnWarning(Action<VoteBundle> action)
        {
            WarningAction = action;
        }


        public void OnFatal(Action<VoteBundle> action)
        {
            ErrorAction = action;
        }



        public int Port { get; private set; }

        public bool ShouldRunScheduler { get; private set; }
        public TimeSpan SchedulerInterval { get; private set; }
        public TimeSpan SchedulerDelay { get; private set; }
        public bool ShouldHostWebServerInProcess { get; private set; }
        public Action<VoteBundle> WarningAction { get; private set; }
        public Action<VoteBundle> ErrorAction { get; private set; }
    }
}