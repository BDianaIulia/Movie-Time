using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string ImdbId { get; set; }
        public string PosterPath { get; set; }
        public string Overview { get; set; }
        public float ImdbVoteAverage { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ICollection<Language> SpokenLanguages { get; set; }
        public ICollection<Genre> Genres { get; set; }
    }
}
