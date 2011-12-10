namespace klinger.Server
{
    using System.Collections.Generic;
    using Messages;

    public interface HealthVoteRepository
    {
        void Save(VoteBundle votes);
        IEnumerable<Vote> All();
    }
}