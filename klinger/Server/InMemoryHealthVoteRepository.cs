namespace klinger.Server
{
    using System;

    public class InMemoryHealthVoteRepository :
        HealthVoteRepository
    {
        public void Save(string name, VoteBundle votes)
        {
            throw new NotImplementedException();
        }

        public void All()
        {
            throw new NotImplementedException();
        }
    }
}