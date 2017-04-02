using System.ComponentModel.DataAnnotations;


namespace webapi.Models
{
    // Need to use properties to for validation to work
    public class NewContact
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNumber { get; set; }
    }
}