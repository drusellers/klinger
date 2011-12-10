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
namespace klinger.Server
{
    using System.Collections.Generic;
    using Messages;
    using Stact;

    public class EnvironmentValidatorRepository : Actor
    {
        readonly ActorInstance _healthRepo;
        readonly List<EnvironmentValidator> _validators = new List<EnvironmentValidator>();

        public EnvironmentValidatorRepository(Inbox inbox, ActorInstance healthRepo)
        {
            _healthRepo = healthRepo;
            inbox.Loop(loop=>
            {
                loop.Receive<TakeTemperature>(HandleTakeTemperature).Continue();
                loop.Receive<AddValidator>(HandleValidator).Continue();
            });
        }

        Consumer<AddValidator> HandleValidator(AddValidator message)
        {
            return msg =>
            {
                _validators.Add(msg.Validator);
            };
        }

        Consumer<TakeTemperature> HandleTakeTemperature(TakeTemperature message)
        {
            return msg =>
            {
                var result = new TemperatureReading() {Votes = TakeTemperature()};
                //send it to the cache;
                _healthRepo.Send(result);
            };
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