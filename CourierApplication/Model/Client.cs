using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CourierApplication.Model
{
    [Table("Clients")]
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        [ForeignKey("Adress")]
        public int AdressId { get; set; }

        public virtual Adress Adress { get; set; }
    }
}
