using eShop.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace eShop.DAL.Data {
    public class eShopDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string> {
        public eShopDbContext(DbContextOptions<eShopDbContext> Options)
            : base(Options) {
        }
        protected override void OnModelCreating(ModelBuilder builder) {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Configuration for Pessimistic locking row 
            foreach (var entityType in builder.Model.GetEntityTypes()){
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)){
                    builder.Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.RowVersion))
                        .IsRowVersion();
                }
            }
            base.OnModelCreating(builder);
        }
        public virtual DbSet<ItemInOrder> ItemInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<PromoCode> PromoCodes { get; set; }
        public virtual DbSet<Visitor>  Visitors { get; set; }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new()) {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified));

            foreach (var entry in entries){

                ((BaseEntity)entry.Entity).ModifiedDate = DateTime.Now;

                if (entry.State == EntityState.Added){ ((BaseEntity)entry.Entity).CreatedDate = DateTime.Now; }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges() {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified));

            foreach (var entry in entries){

                ((BaseEntity)entry.Entity).ModifiedDate = DateTime.Now;

                if (entry.State == EntityState.Added){ ((BaseEntity)entry.Entity).CreatedDate = DateTime.Now; }
            }
            return base.SaveChanges();
        }
    }
}