using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class UserMovieActivity
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public bool Wishlist { get; set; }
        public IdentityUser User { get; set; }
        public Movie Movie { get; set; }
    }
}
