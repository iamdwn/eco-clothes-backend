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
        IGenericRepository<Orderitem> OrderitemRepository { get; }
        IGenericRepository<Payment> PaymentRepository { get; }
        IGenericRepository<Paymentsubscription> PaymentsubscriptionRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Productcategory> ProductcategoryRepository { get; }
        IGenericRepository<Size> SizeRepository { get; }
        IGenericRepository<Sizeproduct> SizeproductRepository { get; }
        IGenericRepository<Subscription> SubscriptionRepository { get; }
        IGenericRepository<User> UserRepository { get; }

        void Save();
    }
}
