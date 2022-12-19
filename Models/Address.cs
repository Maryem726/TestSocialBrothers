using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public class Address
    {
        

        [System.ComponentModel.DataAnnotations.Key]

        [Required]
        public int Id { get; set; }
        [Required]
        public int HouseNumber { get; set; }
        [Required]
        public int ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public string Street { get; set; }




    }
}
