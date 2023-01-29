import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Router } from "@angular/router";
import { map, mergeMap, Observable, switchMap, take } from "rxjs";
import { ResponseModel } from "../../shared/dtos/ResponseModel";
import { Movie } from "../models/movie.model";
import { PaginatedResultModel } from "../models/paginated-result.model";
import { UserMovieActivity } from "../models/user-movie-activity";
import { DataService } from "./data.service";

@Injectable({providedIn: 'root'})
export class MoviesService extends DataService {
  constructor(public http: HttpClient, private router: Router, private snackBar: MatSnackBar) {
    super(http, 'movies');
  }

  public getOverview(): Observable<Array<Movie>> {
    return this.getMany<ResponseModel<Array<Movie>>>('overview').pipe(map((response) => {
        const moviesList = response.data;
        return moviesList;
    }));
  }

  public getMoviesForGenre(genreName: string, pageIndex: number, items: number): Observable<Array<Movie>> {
    return this.getPaginated<ResponseModel<Array<Movie>>>(pageIndex, items, genreName).pipe(map((response) => {
      return response.data;
    }));
  }

  public getMovie(id: string): Observable<Movie> {
    return this.getOneById<ResponseModel<Movie>>(id).pipe(map((response) => {
      return response.data;
    }))
  }

  public getMoviesBySimpleSearch(keywords: string): Observable<Array<Movie>> {
    return this.getMany<ResponseModel<Array<Movie>>>(`simple-search/${keywords}`).pipe(map((response) => {
      return response.data;
    }));
  }

  public getMoviesByAdvancedSearch(keywords: string): Observable<Array<Movie>> {
    return this.getMany<ResponseModel<Array<Movie>>>(`movie-keywords-based-recommender/${keywords}`).pipe(map((response) => {
      return response.data;
    }));
  }

  public getRecommendedUserActivityBasedMovies(): Observable<Array<Movie>> {
    return this.getMany<ResponseModel<Array<Movie>>>(`movie-user-activity-based-recommender`).pipe(map((response) => {
      return response.data;
    }));
  }

  public getRecommendedDescriptionBasedMovies(title: string): Observable<Array<Movie>> {
    return this.getMany<ResponseModel<Array<Movie>>>(`movie-description-based-recommender/${title}`).pipe(map((response) => {
      return response.data;
    }));
  }

  public getRecommendedDetailsBasedMovies(title: string): Observable<Array<Movie>> {
    return this.getMany<ResponseModel<Array<Movie>>>(`movie-details-based-recommender/${title}`).pipe(map((response) => {
      return response.data;
    }));
  }

  public updateMovies(pageIndex: number, items: number) {
    this.getPaginated<ResponseModel<Array<Movie>>>(pageIndex, items).pipe(map((response) => {
        const moviesList = response.data;
        for (const movie of moviesList) {
            this.getRightPosterPath(movie).pipe(take(1)).subscribe((posterPath) => {
                this.update<Movie>(movie.id, posterPath).subscribe();
            });
        }
    })).subscribe();
  }

  public addToWishlist(movie: Movie) {
    const userMovieActivity = new UserMovieActivity();
    userMovieActivity.wishlist = true;
    this.update<UserMovieActivity>(movie.id, userMovieActivity, 'userActivity').subscribe();
    this.snackBar.open("Movie has been added to wishlist", "OK");
  }

  public addRating(movie: Movie, rating: number) {
    const userMovieActivity = new UserMovieActivity();
    userMovieActivity.rating = rating;
    this.update<UserMovieActivity>(movie.id, userMovieActivity, 'userActivity').subscribe();
    this.snackBar.open("Movie has been rated", "OK");
  }
  
  public getRightPosterPath(movie: Movie): Observable<string> {
    return this.http.get<{movie_results: Array<{poster_path: string}>}>(`https://api.themoviedb.org/3/find/${movie.imdbId}?api_key=92fc4e0c7aa11302b4630e16c5ac2752&language=en-US&external_source=imdb_id`)
            .pipe(map((result) => result.movie_results[0].poster_path));
  }

  public moviePercentage(model: Movie) {
    return (model.imdbVoteAverage * 5) / 100;
  }
}