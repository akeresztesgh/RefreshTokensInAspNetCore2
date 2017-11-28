using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string Token { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
