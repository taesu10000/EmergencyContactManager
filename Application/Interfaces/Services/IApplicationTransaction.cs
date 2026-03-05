namespace Infrastructure.Services
{
    public interface IApplicationTransaction
    {
        Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct);
        Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken ct);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}