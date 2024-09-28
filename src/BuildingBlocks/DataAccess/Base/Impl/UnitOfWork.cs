using DataAccess.Models;
using DataAccess.Persistences;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Base.Impl
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private EcoClothesContext context;
        //private GenericRepository<Cart> cartRepository;
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Favorite> favoriteRepository;
        //private GenericRepository<Feedback> feedbackRepository;
        //private GenericRepository<Order> orderRepository;
        //private GenericRepository<Orderitem> orderitemRepository;
        //private GenericRepository<Payment> paymentRepository;
        //private GenericRepository<Paymentsubscription> paymentsubscriptionRepository;
        private GenericRepository<Product> productRepository;
        private GenericRepository<Productcategory> productcategoryRepository;
        private GenericRepository<Size> sizeRepository;
        private GenericRepository<SizeProduct> sizeproductRepository;
        //private GenericRepository<Subscription> subscriptionRepository;
        //private GenericRepository<User> userRepository;

        public UnitOfWork(EcoClothesContext _context)
        {
            context = _context;
        }

        //public IGenericRepository<Cart> CartRepository
        //{
        //    get
        //    {
        //        return cartRepository ??= new GenericRepository<Cart>(context);
        //    }
        //}

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                return categoryRepository ??= new GenericRepository<Category>(context);
            }
        }

        public IGenericRepository<Favorite> FavoriteRepository
        {
            get
            {
                return favoriteRepository ??= new GenericRepository<Favorite>(context);
            }
        }

        //public IGenericRepository<Feedback> FeedbackRepository
        //{
        //    get
        //    {
        //        return feedbackRepository ??= new GenericRepository<Feedback>(context);
        //    }
        //}

        //public IGenericRepository<Order> OrderRepository
        //{
        //    get
        //    {
        //        return orderRepository ??= new GenericRepository<Order>(context);
        //    }
        //}

        //public IGenericRepository<Orderitem> OrderitemRepository
        //{
        //    get
        //    {
        //        return orderitemRepository ??= new GenericRepository<Orderitem>(context);
        //    }
        //}

        //public IGenericRepository<Payment> PaymentRepository
        //{
        //    get
        //    {
        //        return paymentRepository ??= new GenericRepository<Payment>(context);
        //    }
        //}

        //public IGenericRepository<Paymentsubscription> PaymentsubscriptionRepository
        //{
        //    get
        //    {
        //        return paymentsubscriptionRepository ??= new GenericRepository<Paymentsubscription>(context);
        //    }
        //}

        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                return productRepository ??= new GenericRepository<Product>(context);
            }
        }

        public IGenericRepository<Productcategory> ProductcategoryRepository
        {
            get
            {
                return productcategoryRepository ??= new GenericRepository<Productcategory>(context);
            }
        }

        public IGenericRepository<Size> SizeRepository
        {
            get
            {
                return sizeRepository ??= new GenericRepository<Size>(context);
            }
        }

        public IGenericRepository<SizeProduct> SizeproductRepository
        {
            get
            {
                return sizeproductRepository ??= new GenericRepository<SizeProduct>(context);
            }
        }

        //public IGenericRepository<Subscription> SubscriptionRepository
        //{
        //    get
        //    {
        //        return subscriptionRepository ??= new GenericRepository<Subscription>(context);
        //    }
        //}

        //public IGenericRepository<User> UserRepository
        //{
        //    get
        //    {
        //        return userRepository ??= new GenericRepository<User>(context);
        //    }
        //}

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
