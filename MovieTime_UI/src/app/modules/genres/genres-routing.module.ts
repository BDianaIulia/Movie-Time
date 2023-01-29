import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GenreComponent } from './components/genre/genre.component';
import { GenresComponent } from './components/home/genres.component';

const routes: Routes = [
  { path: '', component: GenresComponent },
  { path: 'genre/:name', component: GenreComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GenresRoutingModule {}
