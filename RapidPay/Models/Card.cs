using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RapidPay.Models
{
    public class Card
    {
      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardId { get; set; } 
        public string CardNumber { get; set; }
        
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }

}
