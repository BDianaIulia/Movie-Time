using Application.Interfaces;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System;
using Domain.Entities;
using System.Threading.Tasks;
using Domain.Dtos;
using Domain.Core;
using System.Net.Http;
using System.Linq;

namespace Application.Services
{
    public class SimpleObject
    {
        public string name;
    }
    public class ComplexObject: SimpleObject
    {
        public string iso_639_1;
    }
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageService _languageService;
        private readonly IGenreService _genreService;
        private readonly IRecommendationsClient _recommendationsClient;

        public MovieService(IUnitOfWork unitOfWork, ILanguageService languageService, IGenreService genreService, IRecommendationsClient recommendationsClient)
        {
            _unitOfWork = unitOfWork;
            _languageService = languageService;
            _genreService = genreService;
            _recommendationsClient = recommendationsClient; 
        }
        public async Task<IEnumerable<Movie>> GetOverview()
        {
            return await _unitOfWork.Movies.GetOverview();
        }
        public async Task<IEnumerable<Movie>> GetPaginated(int pageIndex, int itemsPerPage)
        {
            return await _unitOfWork.Movies.GetPaginated(pageIndex, itemsPerPage);
        }
        public async Task<IEnumerable<Movie>> GetPaginatedForGenre(string genreName, int pageIndex, int itemsPerPage)
        {
            return await _unitOfWork.Movies.GetPaginatedForGenre(genreName, pageIndex, itemsPerPage);
        }
        public async Task<Movie> GetItem(Guid movieId)
        {
            return await _unitOfWork.Movies.GetMovie(movieId);
        }
        public async Task<Movie> GetItemByName(string movieName)
        {
            return await _unitOfWork.Movies.GetByName(movieName);
        }
        public async Task<Movie> GetItemByImdbId(string imdbId)
        {
            return await _unitOfWork.Movies.GetByImdbId(imdbId);
        }
        public async Task<int> UpdatePosterPath(Guid movieId, string posterPath)
        {
            var movie = await _unitOfWork.Movies.GetById(movieId);
            if (movie == null)
            {
                return await Task.FromResult(0);
            }

            movie.PosterPath = "https://image.tmdb.org/t/p/original" + posterPath;
            return await _unitOfWork.Movies.Update(movie);
        }
        public async Task<int> UpdateSpokenLanguages(string imdbId, List<Language> spokenLanguages)
        {
            var movie = await _unitOfWork.Movies.GetByImdbId(imdbId);
            if (movie == null)
            {
                return await Task.FromResult(0);
            }

            movie.SpokenLanguages = spokenLanguages;
            return await _unitOfWork.Movies.Update(movie);
        }
        public async Task<int> RegisterMovie(MovieDto movie)
        {
            List<Genre> genres = new List<Genre>();
            foreach (var genre in movie.Genres)
            {
                genres.Add(await _genreService.RegisterGenre(genre));
            }
            List<Language> spokenLanguages = new List<Language>();
            foreach (var language in movie.SpokenLanguages)
            {
                spokenLanguages.Add(await _languageService.RegisterLanguage(language));
            }
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var movieId = Guid.NewGuid();
                    var isTransactionSucceeded = true;
                    
                    var movieToRegister = new Movie
                    {
                        Id = movieId,
                        Title = movie.Title,
                        OriginalTitle = movie.OriginalTitle,
                        ImdbId = movie.ImdbId,
                        PosterPath = movie.PosterPath,
                        Overview = movie.Overview,
                        ImdbVoteAverage = movie.ImdbVoteAverage,
                        ReleaseDate = movie.ReleaseDate,
                        Genres = genres,
                        SpokenLanguages = spokenLanguages
                    };
                    var isMovieRegistered = await _unitOfWork.Movies.Register(movieToRegister);
                    if (isMovieRegistered == 0)
                    {
                        isTransactionSucceeded = false;
                    }
                    if (isTransactionSucceeded)
                    {
                        transaction.Commit();
                        return await Task.FromResult(1);
                    }
                    return await Task.FromResult(0);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return await Task.FromResult(0);
                }
            }
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetMovieDescriptionBasedRecommendations(string movieName)
        {
            return await _recommendationsClient.GetMovieDescriptionBasedRecommendations(movieName);
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetMovieDetailsBasedRecommendations(string movieName)
        {
            return await _recommendationsClient.GetMovieDetailsBasedRecommendations(movieName);
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetMovieUserActivityBasedRecommendations(IEnumerable<Movie> wishListMovies, IEnumerable<Movie> goodRatingMovies)
        {
            var movies = wishListMovies.Concat(goodRatingMovies);
            var movieName = movies.Select(movie => movie.Title).ToList();
            return await _recommendationsClient.GetMovieUserActivityBasedRecommendations(movieName);
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetMovieKeywordsBasedRecommendations(string keywords)
        {
            return await _recommendationsClient.GetMovieKeywordsBasedRecommendations(keywords);
        }
        public async Task<List<Movie>> GetMoviesFilteredByKeywords(string keywords)
        {
            return await _unitOfWork.Movies.GetByFilter(keywords);
        }
        public async Task<List<Movie>> GetMoviesById(List<Guid> ids)
        {
            List<Movie> movies = new List<Movie>();
            foreach (var id in ids)
            {
                var movie = await _unitOfWork.Movies.GetById(id);
                if (movie == null)
                    continue;
                movies.Add(movie);
            }
            return movies;
        }
        public async Task<List<Movie>> GetMoviesByImdbId(List<string> ids)
        {
            List<Movie> movies = new List<Movie>();
            foreach (var id in ids)
            {
                var movie = await GetItemByImdbId(id);
                if (movie == null)
                    continue;
                movies.Add(movie);
            }
            return movies;
        }
        public async Task<List<Movie>> GetMoviesByName(List<string> titles)
        {
            List<Movie> movies = new List<Movie>();
            foreach (var title in titles)
            {
                var movie = await GetItemByName(title);
                if (movie == null)
                    continue;
                movies.Add(movie);
            }
            return movies;
        }
        public async Task<bool> LoadMoviesMetadataAsync()
        {
            var path = @"../movies_metadata.csv";
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                List<string> headers = new List<string>(csvParser.ReadFields());
                while (!csvParser.EndOfData)
                {
                    List<string> fields = new List<string>(csvParser.ReadFields());
                    string genresObject = fields[position(headers, "genres")];
                    string imdbIdObject = fields[position(headers, "imdb_id")];
                    string originalTitleObject = fields[position(headers, "original_title")];
                    string titleObject = fields[position(headers, "title")];
                    string overviewObject = fields[position(headers, "overview")];
                    string posterPathObject = "https://image.tmdb.org/t/p/original/" + fields[position(headers, "poster_path")];
                    string releaseDateObject = fields[position(headers, "release_date")];
                    string spokenLanguagesObject = fields[position(headers, "spoken_languages")];
                    string voteAverageObject = fields[position(headers, "vote_average")];

                    if (releaseDateObject.Length == 0) continue;
                    List<Genre> genres = genresList(genresObject);
                    List<Language> spokenLanguagesDto = languagesList(spokenLanguagesObject);
                    DateTime releaseDate = DateTime.Parse(releaseDateObject);

                    var MovieDto = new MovieDto
                    {
                        Title = titleObject,
                        OriginalTitle = originalTitleObject,
                        ImdbId = imdbIdObject,
                        PosterPath = posterPathObject,
                        Overview = overviewObject,
                        ImdbVoteAverage = float.Parse(voteAverageObject),
                        ReleaseDate = releaseDate,
                        SpokenLanguages = mapSpokenLanguagesToDtos(spokenLanguagesDto),
                        Genres = mapGenresToDtos(genres)
                    };
                    await RegisterMovie(MovieDto);
                }
            }
            return true;
        }
        private int position(List<string> list, string searchedItem)
        {
            return list.FindIndex(str => str.Equals(searchedItem));
        }
        private List<Genre> genresList(string genresObject)
        {
            List<SimpleObject> genres = (List<SimpleObject>)DeserializeToList<SimpleObject>(genresObject);
            return genres.ConvertAll(genreObj =>
            { 
                Genre genre = new Genre();
                genre.Name = genreObj.name;
                return genre;
            });
        }
        private List<Language> languagesList(string languagesObject)
        {
            List<ComplexObject> languages = (List<ComplexObject>)DeserializeToList<ComplexObject>(languagesObject);
            return languages.ConvertAll(languageObj => 
            { 
                Language language = new Language();
                language.Name = languageObj.name;
                language.Code = languageObj.iso_639_1;
                return language;
            });
        }
        private static IList<T> DeserializeToList<T>(string jsonString)
        {
            try
            {
                var array = JArray.Parse(jsonString);
                IList<T> objectsList = new List<T>();

                foreach (var item in array)
                {
                    try
                    {
                        objectsList.Add(item.ToObject<T>());
                    }
                    catch (Exception ex)
                    { }
                }
                return objectsList;
            }
            catch(Exception ex)
            {
                return new List<T>();
            }
        }
        private ICollection<LanguageDto> mapSpokenLanguagesToDtos(List<Language> spokenLanguages)
        {
            ICollection<LanguageDto> languages = new List<LanguageDto>();
            foreach (var language in spokenLanguages)
            {
                languages.Add(new LanguageDto
                {
                    Code = language.Code,
                    Name = language.Name
                });
            }
            return languages;
        }
        private ICollection<GenreDto> mapGenresToDtos(List<Genre> genres)
        {
            ICollection<GenreDto> genreDtos = new List<GenreDto>();
            foreach (var genre in genres)
            {
                genreDtos.Add(new GenreDto
                {
                    Name = genre.Name
                });
            }
            return genreDtos;
        }
    }
}
