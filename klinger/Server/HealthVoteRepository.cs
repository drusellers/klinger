namespace klinger.Server
{
    public interface HealthVoteRepository
    {
        void Save(string name, VoteBundle votes);
        void All();
    }
}