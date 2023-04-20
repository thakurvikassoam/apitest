using System.ComponentModel.DataAnnotations;

namespace CFAWebApi.Models
{
    public class TicketHistory
    {
        [Key]
        public int TicketId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }   
        public string UpdateDate { get; set; }  
        public string UpdateBy { get; set; }  

    }
}
