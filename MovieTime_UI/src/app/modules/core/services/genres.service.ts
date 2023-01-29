import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { map, mergeMap, Observable, switchMap, take } from "rxjs";
import { ResponseModel } from "../../shared/dtos/ResponseModel";
import { Genre } from "../models/genre.model";
import { Movie } from "../models/movie.model";
import { PaginatedResultModel } from "../models/paginated-result.model";
import { DataService } from "./data.service";

@Injectable({providedIn: 'root'})
export class GenresService extends DataService {
  constructor(public http: HttpClient, private router: Router) {
    super(http, 'genres');
  }

  public getGenres(): Observable<Array<Genre>> {
    return this.getAll<ResponseModel<Array<Genre>>>().pipe(map((response) => {
        const genresList = response.data;
        return genresList;
    }));
  }

  public getGenreNames(): Observable<Array<string>> {
    return this.getMany<ResponseModel<Array<string>>>(`list`).pipe(map((response) => {
      return response.data;
    }))
  }

  public getGenreRecommendations(genreName: string): Observable<Array<Movie>> {
    return this.getMany<ResponseModel<Array<Movie>>>(`genre-recommendations/${genreName}`).pipe(map((response) => {
      return response.data;
    }))
  }
}