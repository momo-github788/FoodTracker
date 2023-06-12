namespace backend.Repository {
    public interface IUnitOfWork : IDisposable {
        FoodRecordRepository FoodRecords { get; }
        int Save();

    }
}
