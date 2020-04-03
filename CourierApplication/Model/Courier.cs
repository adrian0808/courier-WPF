using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CourierApplication.Model
{
    [Table("Couriers")]
    public class Courier
    {
        [Key]
        public int CourierId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isFree { get; set; }
    }
}
