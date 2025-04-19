using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BmdbNetWeb.Models {
    [Table("MovieCollection")]
    public partial class MovieCollection {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
