using System;
using System.Threading;
using Stact;
using klinger.Server;
using klinger.Server.Messages;

namespace klinger.specs
{
    public class Bob
    {
        public void should_call_vote_on_validators()
        {
            ActorInstance healthRepo = null;
            var af = ActorFactory.Create(inbox => new EnvironmentValidatorRepository(inbox, healthRepo));

            var r = af.GetActor();

            var testValidator = new TestValidator();
            r.Send(new AddValidator
                   {
                       Validator = testValidator
                   });

            r.Send<TakeTemperature>();

            Thread.Sleep(1000);
            Console.WriteLine(testValidator.VoteCalled);
        }



        class TestValidator : EnvironmentValidator
        {
            public bool VoteCalled;

            public string SystemName
            {
                get { return "Test"; }
            }

            public void Vote(Ballot ballot)
            {
                VoteCalled = true;
            }
        }
    }
}