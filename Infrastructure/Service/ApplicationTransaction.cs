namespace Infrastructure.Services
{
    public sealed class ApplicationTransaction : IApplicationTransaction
    {
        private readonly DBContext dBContext;

        public ApplicationTransaction(DBContext dBContext)
            => this.dBContext = dBContext;

        public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct)
        {
            if (dBContext.Database.CurrentTransaction is not null)
            {
                return await action(ct);
            }

            await using var tx = await dBContext.Database.BeginTransactionAsync(ct);
            try
            {
                var result = await action(ct);
                await dBContext.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
                return result;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
        public async Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken ct)
        {
            if (dBContext.Database.CurrentTransaction is not null)
            {
                await action(ct);
                return;
            }

            await using var tx = await dBContext.Database.BeginTransactionAsync(ct);
            try
            {
                await action(ct);
                await dBContext.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
        public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await dBContext.SaveChangesAsync(ct);
    }
}
