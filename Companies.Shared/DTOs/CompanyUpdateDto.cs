﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.Shared.DTOs
{
    public record CompanyUpdateDto : CompanyForManipulationDto
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Company name is a required field.")]
        //[MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        //public string? Name { get; set; }

        //[Required(ErrorMessage = "Company address is a required field.")]
        //[MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters")]
        //public string? Address { get; set; }
        //public string? Country { get; set; }
    }
}
