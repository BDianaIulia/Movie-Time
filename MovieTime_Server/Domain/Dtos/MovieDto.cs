using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dtos
{
    public class MovieDto
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string ImdbId { get; set; }
        public string PosterPath { get; set; }
        public string Overview { get; set; }
        public float ImdbVoteAverage { get; set; }
        public DateTime ReleaseDate { get; set; }
        public LanguageDto OriginalLanguage { get; set; }
        public ICollection<LanguageDto> SpokenLanguages { get; set; }
        public ICollection<GenreDto> Genres { get; set; }
    }
}
