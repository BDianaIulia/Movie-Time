import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieComponent } from './components/home/movie.component';
import { SharedModule } from '../shared/shared.module';
import { MovieRoutingModule } from './movie-routing.module';

@NgModule({
  declarations: [MovieComponent],
  imports: [CommonModule, SharedModule, MovieRoutingModule],
})
export class MovieModule {}
