namespace klinger.Server
{
    using System;
    using System.Collections.Generic;
    using Magnum.Extensions;
    using Messages;
    using Stact;

    public class InMemoryHealthVoteRepository :
        HealthVoteRepository,
        Actor
    {
        IList<Vote> _votes;

        public InMemoryHealthVoteRepository(Inbox inbox)
        {
            _votes = new List<Vote>();

            inbox.Loop(loop=>
            {
                loop.Receive<TemperatureReading>(HandleNewReading).Continue();
                
            });
        }

        Consumer<TemperatureReading> HandleNewReading(TemperatureReading message)
        {
            //conditional check here

            return msg => Save(new VoteBundle(msg.Votes));
        }

        public void Save(VoteBundle votes)
        {
            votes.Votes.Each(vote =>
            {
                _votes.Add(vote);
            });
        }

        public IEnumerable<Vote> All()
        {
            return _votes;
        }
    }
}