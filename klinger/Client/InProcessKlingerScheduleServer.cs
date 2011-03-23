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
    using System.Linq;
    using Magnum.Extensions;
    using Stact;
    using Stact.Internal;

    public class InProcessKlingerScheduleServer
    {
        readonly Scheduler _scheduler;
        readonly Fiber _fiber;
        readonly EnvironmentValidatorRepository _repository;
        readonly TimeSpan _schedulerDelay;
        readonly TimeSpan _schedulerInterval;
        public UntypedChannel EventChannel;

        public InProcessKlingerScheduleServer(EnvironmentValidatorRepository repository, TimeSpan schedulerDelay,
                                              TimeSpan schedulerInterval)
        {
            EventChannel = new ChannelAdapter();

            _fiber = new PoolFiber();
            _scheduler = new TimerScheduler(_fiber);
            _repository = repository;
            _schedulerDelay = schedulerDelay;
            _schedulerInterval = schedulerInterval;
        }

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
            _scheduler.Schedule(_schedulerDelay, _schedulerInterval, _fiber, () =>
            {
                var x = _repository.TakeTemperature();

                EventChannel.Send(new VoteBundle(x));
            });
        }

        public void Stop()
        {
            _scheduler.Stop(5.Seconds());
        }
    }
}