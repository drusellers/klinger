namespace SampleApplication
{
    using System;
    using hawkeye;
    using hawkeye.Config;

    class Program
    {
        static void Main(string[] args)
        {
            HawkeyeConfigurator.Configure(cfg=>
            {
                cfg.RegisterValidator<MyCheck>();
                cfg.RegisterValidator<CheckTwo>();

                cfg.HostInProcess(8008);
            });

            Console.ReadKey(true);
        }
    }

    public class MyCheck :
        EnvironmentValidator
    {
        public void Vote(Vote vote)
        {
            if(DateTime.Now.Second % 2 == 0)
            {
                vote.Fatal("ERROR");
                return;
            }

            vote.Healthy();
        }
    }
    public class CheckTwo :
        EnvironmentValidator
    {
        public void Vote(Vote vote)
        {
            if(DateTime.Now.Second % 3 == 0)
            {
                vote.Fatal("ERROR");
                return;
            }

            vote.Healthy();
        }
    }
}
