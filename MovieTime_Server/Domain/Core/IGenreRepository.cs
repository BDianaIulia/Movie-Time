using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core
{
    public interface IGenreRepository: IRepository<Genre>
    {
        Task<Genre?> GetGenreByName(string genreName);
    }
}
