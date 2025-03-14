﻿using System.ComponentModel.DataAnnotations;

namespace Inzynierka.Application.DTOs
{
    public class LoginContractorDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
