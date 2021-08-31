using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTPNS.Web.Models
{
    public class ImportViewModel
    {
        [Display(Name = "UploadFile")]
        [Required]
        public IFormFile ExcelFile { get; set; }
        public Tuple<List<string>, List<List<string>>> Datas { get; set; }
        public string FileName { get; set; }
    }
}
