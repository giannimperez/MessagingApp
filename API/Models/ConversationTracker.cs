using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ConversationTracker
    {
        [Key]
        public int Id { get; set; }
        public string UserA { get; set; }
        public string UserB { get; set; }
        public DateTime MostRecentMessageDate { get; set; }
    }
}
