using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static zero_net_core.Core.Enums;

namespace zero_net_core.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Trigram")]
        public string Trigram { get; set; }
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        public string GenderName { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        public List<SelectListItem> Genders { get; set; }
    }
}
