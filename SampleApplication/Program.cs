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
namespace SampleApplication
{
    using System;
    using hawkeye;
    using hawkeye.Config;
    using Magnum.Extensions;

    internal class Program
    {
        static void Main(string[] args)
        {
            HawkeyeConfigurator.Configure(cfg =>
            {
                //now with type scanning
                cfg.RegisterAllValidatorsInAssembly<MyCheck>();

                //or
                //cfg.RegisterValidator<MyCheck>();
                //cfg.RegisterValidator<CheckTwo>();

                //loads the timer based scheduler
                cfg.ScheduleValidations(1.Seconds(), 1.Seconds());

                //loads the web server in process
                cfg.HostWebServerInProcess(8008); // this is the web server

                //action to take when one of the votes is a warning
                cfg.OnWarning(allVotes =>
                {
                    Console.WriteLine("warning");
                });


                //action to take when one of the votes is a fatal
                cfg.OnFatal(allVotes =>
                {
                    Console.WriteLine("fatal");
                });

                //Coming Soon
                //cfg.ForwardTo(new Uri("http://localhost:8008/hawkeye/report"));
            });

            Console.ReadKey(true);
        }
    }

    public class MyCheck :
        EnvironmentValidator
    {
        public void Vote(Ballot ballot)
        {
            if (DateTime.Now.Second%2 == 0)
            {
                ballot.Fatal("ERROR");
                return;
            }

            ballot.Healthy();
        }
    }

    public class CheckTwo :
        EnvironmentValidator
    {
        public void Vote(Ballot ballot)
        {
            if (DateTime.Now.Second%3 == 0)
            {
                ballot.Fatal("ERROR");
                return;
            }

            ballot.Healthy();
        }
    }
}