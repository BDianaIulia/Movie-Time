using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dtos
{
    public class UserMovieActivityDto
    {
        public int Rating { get; set; }
        public bool Wishlist { get; set; }
    }
}
