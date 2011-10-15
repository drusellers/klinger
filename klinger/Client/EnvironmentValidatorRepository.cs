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
namespace klinger.Client
{
    using System;
    using System.Collections.Generic;
    using Magnum.Reflection;
    using Stact;

    public class EnvironmentValidatorRepository : Actor
    {
        readonly Inbox _inbox;
        readonly List<EnvironmentValidator> _validators = new List<EnvironmentValidator>();

        public EnvironmentValidatorRepository(Inbox inbox)
        {
            _inbox = inbox;
        }

        public void AddCheck(EnvironmentValidator validator)
        {
            _validators.Add(validator);
        }

        public void AddCheck<THealthCheck>() where THealthCheck : EnvironmentValidator, new()
        {
            AddCheck(new THealthCheck());
        }

        public void AddCheck(Type t)
        {
            AddCheck(FastActivator.Create(t) as EnvironmentValidator);
        }

        public ValidationVote[] TakeTemperature()
        {
            var votes = new List<ValidationVote>();
            foreach (var environmentValidator in _validators)
            {
                var vote = new ValidationVote(environmentValidator.SystemName);
                environmentValidator.Vote(vote);
                votes.Add(vote);
            }
            return votes.ToArray();
        }
    }
}