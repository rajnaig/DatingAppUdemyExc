﻿using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class LoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}