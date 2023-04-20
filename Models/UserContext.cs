using Microsoft.EntityFrameworkCore;

namespace CFAWebApi.Models
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions options) : base(options) { }
        DbSet<UserData> UserData
        {
            get;
            set;
        }
        DbSet<Tickets> TicketInfo
        {
            get;
            set;
        }
        DbSet<TicketHistory> TicketHistory
        {
            get;
            set;
        }

    }
}
