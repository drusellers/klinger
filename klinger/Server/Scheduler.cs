// Copyright 2007-2011 The Apache Software Foundation.
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
    using Magnum.Extensions;
    using Stact;
    using Stact.Internal;

    public interface Scheduler : Actor
    {
    }

    //in the beginning there is only one schedule.
    public class CentralScheduler : Scheduler
    {
        readonly ActorInstance _repository;
        readonly Fiber _fiber;
        readonly Stact.Scheduler _scheduler;


        public CentralScheduler(Inbox inbox, ActorInstance repository, Fiber fiber)
        {
            _repository = repository;
            _fiber = fiber;
            _scheduler = new TimerScheduler(_fiber);

            inbox.Loop(loop=>
            {
                loop.Receive<StartIt>(HandleStart);
                loop.Receive<StopIt>(HandleStop);
            });
        }

        Consumer<StartIt> HandleStart(StartIt message)
        {
            //func<t, action<t>>
            return msg => _scheduler.Schedule(msg.Delay, msg.Interval, _fiber, () =>
            {
                _repository.Send<TakeTemperature>();
            });
        }

        Consumer<StopIt> HandleStop(StopIt message)
        {
            //func<t, action<t>>

            return msg =>
            {
                _scheduler.Stop(5.Seconds());
            };
        }
    }

    public class StartIt
    {
        public TimeSpan Delay { get; set; }
        public TimeSpan Interval { get; set; }
    }
    public class StopIt
    {
        
    }
}