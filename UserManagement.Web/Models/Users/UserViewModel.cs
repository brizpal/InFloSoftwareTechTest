﻿using System;
using System.ComponentModel.DataAnnotations;


namespace UserManagement.Web.Models.Users;
public class UserViewModel
{
    public long Id { get; set; }

    [Required]
    public string? Forename { get; set; }

    [Required]
    public string? Surname { get; set; }

    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateOnly DateOfBirth { get; set; }
    public bool IsActive { get; set; }
}
