using CourierApplication.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CourierApplication.Model
{
    [Table("Adresses")]
    public class Adress
    {
        [Key]
        public int AdressId { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal Longitude { get; set; }

        public ICollection<Client> Client{ get; set; }

    }
}
