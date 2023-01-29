using Application.Interfaces;
using Domain.Core;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MovieGenreService: IMovieGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovieGenreService(IUnitOfWork unitOfWork, 
            IMovieService movieService, 
            IGenreService genreService)
        {
            _unitOfWork = unitOfWork;
        }
        //public async void AddMovieGenre(Movie movie, Genre genre)
        //{
        //    try
        //    {
        //        //Movie movie = await _unitOfWork.Movies.GetById(newCharacterSkill.MovieId);
        //        //    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
        //        //    c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
        //        //if (character == null)
        //        //{
        //        //    //response.Success = false;
        //        //    //response.Message = "Character not found.";
        //        //    return response;
        //        //}
        //        //Genre skill = await _context.Skills
        //        //    .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
        //        //if (skill == null)
        //        //{
        //        //    response.Success = false;
        //        //    response.Message = "Skill not found.";
        //        //    return response;
        //        //}
        //        //CharacterSkill characterSkill = new CharacterSkill
        //        //{
        //        //    Character = character,
        //        //    Skill = skill
        //        //};

        //        //await _context.CharacterSkills.AddAsync(characterSkill);
        //        //await _context.SaveChangesAsync();
        //        //response.Data = _mapper.Map<GetCharacterDto>(character);
        //    }
        //    catch (Exception ex)
        //    {}
        //    return response;
        //}
    }
}
