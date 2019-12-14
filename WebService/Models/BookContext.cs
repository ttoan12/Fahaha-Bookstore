using System.Data.Entity;

namespace WebService.Models
{
    public partial class BookContext : DbContext
    {
        public BookContext() : base()
        {
            Database.Connection.ConnectionString = "workstation id=da2-16dh110070.mssql.somee.com;packet size=4096;user id=huflit16dh110070_SQLLogin_1;pwd=xox4aha89z;data source=da2-16dh110070.mssql.somee.com;persist security info=False;initial catalog=da2-16dh110070";
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<BookType> BookTypes { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Watched> Watcheds { get; set; }

        public DbSet<SachTop5> SachTop5s { get; set; }
    }
}