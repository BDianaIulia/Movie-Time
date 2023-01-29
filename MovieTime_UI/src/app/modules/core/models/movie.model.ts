import { Genre } from "./genre.model";
import { MovieLanguage } from "./movie-language.model";

export interface Movie {
    id: string;
    title: string;
    originalTitle: string;
    imdbId: string;
    posterPath: string;
    overview: string;
    imdbVoteAverage: number;
    releaseDate: Date;
    spokenLanguages: Array<MovieLanguage>;
    genres: Array<Genre>;
}