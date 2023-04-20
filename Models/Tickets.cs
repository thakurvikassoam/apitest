using System.ComponentModel.DataAnnotations;

namespace CFAWebApi.Models
{
    public class Tickets
    {
        [Key]
        public int Id { get; set; }        
        public string Title { get; set; }
        public string Description { get; set; }
        public string status  { get; set; }
        public string  Createdate { get; set; }
        public string  Updatedate { get; set; }
        public string  Updatedby { get; set; }
        public string CreatedBy { get; set; }
        public string Reply { get; set; }
        public string ReplyBy { get; set; }
        public string Priority { get; set; }
        public string FileAttachment { get; set; }

    }
}
