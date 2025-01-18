using DataAccess.Models;
using DataAccess.Persistences;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Base.Impl
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private EcoClothesContext context;
        private IDbContextTransaction _transaction;

        private GenericRepository<Cart> cartRepository;
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Favorite> favoriteRepository;
        private GenericRepository<Feedback> feedbackRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<OrderItem> orderitemRepository;
        private GenericRepository<Payment> paymentRepository;
        private GenericRepository<PaymentSubscription> paymentsubscriptionRepository;
        private GenericRepository<Product> productRepository;
        private GenericRepository<ProductCategory> productcategoryRepository;
        private GenericRepository<Size> sizeRepository;
        private GenericRepository<SizeProduct> sizeproductRepository;
        private GenericRepository<Subscription> subscriptionRepository;
        private GenericRepository<User> userRepository;

        public UnitOfWork(EcoClothesContext _context)
        {
            context = _context;
        }

        public IGenericRepository<Cart> CartRepository => cartRepository ??= new GenericRepository<Cart>(context);
        public IGenericRepository<Category> CategoryRepository => categoryRepository ??= new GenericRepository<Category>(context);
        public IGenericRepository<Favorite> FavoriteRepository => favoriteRepository ??= new GenericRepository<Favorite>(context);
        public IGenericRepository<Feedback> FeedbackRepository => feedbackRepository ??= new GenericRepository<Feedback>(context);
        public IGenericRepository<Order> OrderRepository => orderRepository ??= new GenericRepository<Order>(context);
        public IGenericRepository<OrderItem> OrderitemRepository => orderitemRepository ??= new GenericRepository<OrderItem>(context);
        public IGenericRepository<Payment> PaymentRepository => paymentRepository ??= new GenericRepository<Payment>(context);
        public IGenericRepository<PaymentSubscription> PaymentsubscriptionRepository => paymentsubscriptionRepository ??= new GenericRepository<PaymentSubscription>(context);
        public IGenericRepository<Product> ProductRepository => productRepository ??= new GenericRepository<Product>(context);
        public IGenericRepository<ProductCategory> ProductcategoryRepository => productcategoryRepository ??= new GenericRepository<ProductCategory>(context);
        public IGenericRepository<Size> SizeRepository => sizeRepository ??= new GenericRepository<Size>(context);
        public IGenericRepository<SizeProduct> SizeproductRepository => sizeproductRepository ??= new GenericRepository<SizeProduct>(context);
        public IGenericRepository<Subscription> SubscriptionRepository => subscriptionRepository ??= new GenericRepository<Subscription>(context);
        public IGenericRepository<User> UserRepository => userRepository ??= new GenericRepository<User>(context);

        public void Save()
        {
            var validationErrors = context.ChangeTracker.Entries<IValidatableObject>()
                .SelectMany(e => e.Entity.Validate(null))
                .Where(e => e != ValidationResult.Success)
                .ToArray();
            if (validationErrors.Any())
            {
                var exceptionMessage = string.Join(Environment.NewLine,
                    validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
                throw new Exception(exceptionMessage);
            }
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
