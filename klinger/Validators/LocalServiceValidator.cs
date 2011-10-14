namespace klinger.Validators
{
    using System.ServiceProcess;
    using Magnum.Extensions;

    public class LocalServiceValidator : 
        EnvironmentValidator
    {
        string _serviceName;

        public LocalServiceValidator(string name)
        {
            _serviceName = name;
        }

        public string SystemName
        {
            get { return _serviceName; }
        }

        public void Vote(Ballot ballot)
        {
            using(var c = new ServiceController(_serviceName))
            {
                if(c.Status != ServiceControllerStatus.Running)
                {
                    ballot.Fatal("Service Controller reports '{0}' is not in the running state.".FormatWith(_serviceName));
                    return;
                }
                ballot.Healthy();
            }
        }
    }
}