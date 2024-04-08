namespace FleetManagement.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
