using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Language
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
