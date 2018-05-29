using Microsoft.EntityFrameworkCore;
using paypart_item_gateway.Models;

namespace paypart_item_gateway.Services
{
    public class ItemSqlServerContext : DbContext
    {
        public ItemSqlServerContext(DbContextOptions<ItemSqlServerContext> options) : base(options)
        {

        }

        public DbSet<CostItem> CostItem { get; set; }
        public DbSet<FormItem> FormItem { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CostItem>().ToTable("CostItem");
            modelBuilder.Entity<FormItem>().ToTable("FormItem");

        }
    }
}
