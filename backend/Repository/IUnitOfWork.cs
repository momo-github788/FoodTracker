namespace backend.Repository {
    public interface IUnitOfWork : IDisposable {
        FoodRecordRepository FoodRecords { get; }
        ConfirmationTokenRepository ConfirmationTokens { get; }
        int Save();

    }
}
