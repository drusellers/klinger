namespace klinger.Server
{
    using Messages;

    public interface HealthVoteRepository
    {
        void Save(string name, VoteBundle votes);
        void All();
    }
}