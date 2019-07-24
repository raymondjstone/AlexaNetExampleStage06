using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestyMcTestFaceAuth.Models
{
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string guid { get; set; }
        public string amazonclientid { get; set; }
        public string amazonresponsetype { get; set; }
        public string amazonstate { get; set; }
        public string amazonredirecturi { get; set; }

    }
}
