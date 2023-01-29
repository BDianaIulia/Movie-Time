import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { GenresComponent } from './components/home/genres.component';
import { GenresRoutingModule } from './genres-routing.module';
import { GenreComponent } from './components/genre/genre.component';

@NgModule({
  declarations: [GenresComponent, GenreComponent],
  imports: [CommonModule, SharedModule, GenresRoutingModule],
})
export class GenresModule {}
