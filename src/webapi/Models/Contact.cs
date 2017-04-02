using System;
using System.ComponentModel.DataAnnotations;


namespace webapi.Models
{
    // Need to use properties to for validation to work
    public class Contact : NewContact
    {
        [Required]
        public Guid Id { get; set; }
    }
}