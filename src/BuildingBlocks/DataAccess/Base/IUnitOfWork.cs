using DataAccess.Models;

namespace DataAccess.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Cart> CartRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Favorite> FavoriteRepository { get; }
        IGenericRepository<Feedback> FeedbackRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<OrderItem> OrderitemRepository { get; }
        IGenericRepository<Payment> PaymentRepository { get; }
        IGenericRepository<PaymentSubscription> PaymentsubscriptionRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<ProductCategory> ProductcategoryRepository { get; }
        IGenericRepository<Size> SizeRepository { get; }
        IGenericRepository<SizeProduct> SizeproductRepository { get; }
        IGenericRepository<Subscription> SubscriptionRepository { get; }
        IGenericRepository<User> UserRepository { get; }

        void Save();
        Task CommitAsync();
        Task RollbackAsync();
        Task BeginTransactionAsync();
    }
}
