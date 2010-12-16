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
    using System.Collections.Generic;
    using Magnum.Reflection;

    public class HealthRepository
    {
        readonly List<EnvironmentValidator> _validators = new List<EnvironmentValidator>();


        public void AddCheck<THealthCheck>() where THealthCheck : EnvironmentValidator, new()
        {
            _validators.Add(new THealthCheck());
        }

        public void AddCheck(Type t)
        {
            _validators.Add(FastActivator.Create(t) as EnvironmentValidator);
        }

        public ValidationVote[] TakeTemperature()
        {
            var votes = new List<ValidationVote>();
            foreach (var environmentValidator in _validators)
            {
                var vote = new ValidationVote(environmentValidator.GetType().Name);
                environmentValidator.Vote(vote);
                votes.Add(vote);
            }
            return votes.ToArray();
        }
    }
}