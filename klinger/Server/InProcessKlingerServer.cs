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
    using System;
    using System.Linq;
    using Messages;
    using Stact;

    public class InProcessKlingerServer :
        KlingerServer
    {
        readonly TimeSpan _schedulerDelay;
        readonly TimeSpan _schedulerInterval;
        readonly Fiber _fiber;

        public InProcessKlingerServer(TimeSpan schedulerDelay,
                                              TimeSpan schedulerInterval)
        {
            _schedulerDelay = schedulerDelay;
            _schedulerInterval = schedulerInterval;

            _fiber = new PoolFiber();
        }

        public UntypedChannel EventChannel;
        ActorInstance _scheduleActor;
        ActorInstance _repository;

        public void OnFatal(Action<VoteBundle> action)
        {
            if (action == null) return;

            EventChannel.Connect(c => c.AddConsumerOf<VoteBundle>().UsingConsumer(vb=>
            {
                if (vb.Votes.Any(v => v.State == HealthState.Fatal))
                    action(vb);
            }).HandleOnFiber(_fiber));
        }

        public void OnWarning(Action<VoteBundle> action)
        {
            if (action == null) return;

            EventChannel.Connect(c => c.AddConsumerOf<VoteBundle>().UsingConsumer(vb => 
            { 
                if(vb.Votes.Any(v=>v.State ==HealthState.Warning))
                    action(vb);
            }).HandleOnFiber(_fiber));
        }

        public void Start()
        {
            var repoFactory = ActorFactory.Create(inbox => new EnvironmentValidatorRepository(inbox));
            _repository = repoFactory.GetActor();

            var schedulerFactory = ActorFactory.Create((fiber,inbox) => new CentralScheduler(inbox, _repository, fiber));
            _scheduleActor = schedulerFactory.GetActor();


            _scheduleActor.Send(new StartIt()
                {
                    Interval = _schedulerInterval,
                    Delay = _schedulerDelay
                });
        }

        public void Stop()
        {
            _scheduleActor.Send<StopIt>();
            _scheduleActor.Exit();

            _repository.Exit();
        }
    }
}