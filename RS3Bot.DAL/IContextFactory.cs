namespace RS3Bot.DAL
{
    public interface IContextFactory
    {
        ApplicationDbContext Create();
    }
}
