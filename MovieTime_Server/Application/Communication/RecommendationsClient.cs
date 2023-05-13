using Application.Interfaces;
using Domain.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Application.Communication
{
    public class RecommendationsClient: IRecommendationsClient
    {
        private HttpClient _httpClient;
        public RecommendationsClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5000/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetGenreRecommendations(string genreName)
        {
            using (var response = await _httpClient.GetAsync($"/api/genre-recommender?genre={genreName}"))
            {
                try
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(new List<RecommendedMovieDto>());
                    var recommendationsResponse = JsonConvert.DeserializeObject<RecommendationsResponse<RecommendedMovieDto>>(apiResponse);
                    return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(recommendationsResponse.data);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetMovieDescriptionBasedRecommendations(string movieName)
        {
            using (var response = await _httpClient.GetAsync($"/api/movie-description-based-recommender?movie_title={movieName}"))
            {
                try
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(new List<RecommendedMovieDto>());
                    var recommendationsResponse = JsonConvert.DeserializeObject<RecommendationsResponse<RecommendedMovieDto>>(apiResponse);
                    return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(recommendationsResponse.data);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetMovieDetailsBasedRecommendations(string movieName)
        {
            using (var response = await _httpClient.GetAsync($"/api/movie-details-based-recommender?movie_title={movieName}"))
            {
                try
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(new List<RecommendedMovieDto>());
                    var recommendationsResponse = JsonConvert.DeserializeObject<RecommendationsResponse<RecommendedMovieDto>>(apiResponse);
                    return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(recommendationsResponse.data);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetMovieKeywordsBasedRecommendations(string keywords)
        {
            using (var response = await _httpClient.GetAsync($"/api/movie-keywords-based-recommender?keywords={keywords}"))
            {
                try
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(new List<RecommendedMovieDto>());
                    var recommendationsResponse = JsonConvert.DeserializeObject<RecommendationsResponse<RecommendedMovieDto>>(apiResponse);
                    return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(recommendationsResponse.data);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
        public async Task<IEnumerable<RecommendedMovieDto>> GetMovieUserActivityBasedRecommendations(IEnumerable<string> moviesTitle)
        {
            var client = new RestClient("http://localhost:5000/");
            var request = new RestRequest("/api/movie-wishlist-based-recommender", Method.Post);
            request.AddHeader("Content-Type", "application/json");

            var json = JsonConvert.SerializeObject(new { movie_titles = moviesTitle });
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            try
            {
                var clientValue = client.Execute<RecommendationsResponse<RecommendedMovieDto>>(request);
                if (clientValue.StatusCode != System.Net.HttpStatusCode.OK)
                    return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(new List<RecommendedMovieDto>());
                var recommendationsResponse = JsonConvert.DeserializeObject<RecommendationsResponse<RecommendedMovieDto>>(clientValue.Content);
                return await Task.FromResult<IEnumerable<RecommendedMovieDto>>(recommendationsResponse.data);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
