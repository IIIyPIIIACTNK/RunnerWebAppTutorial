﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace RunWebAppTutorial.Models
{
    public class AppUser : IdentityUser
    {
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        [ForeignKey ("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<Club> Clubs { get; set; }
        public ICollection<Race> Races { get; set; }

    }
}
