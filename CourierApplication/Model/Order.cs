using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CourierApplication.Model
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("Adress")]
        public int AdressId { get; set; }
        public bool isCompleted { get; set; }

        public virtual Adress Adress { get; set; }
    }
}
