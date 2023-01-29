import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HoverClassDirective } from './directives/hover-class.directive';
import { PaginatorComponent } from './components/paginator/paginator.component';
import { MaterialModule } from 'src/material.module';
import { SidenavResponsiveComponent } from './components/sidenav/sidenav-responsive.component';
import { SidenavService } from './services/sidenav.service';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule } from '@angular/router';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { TopNavigationBarComponent } from './components/top-navigaton-bar/top-navigation-bar.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MoviesCardComponent } from './movies-card/movies-card.component';
import { MoviesCarouselComponent } from './components/movies-carousel/movies-carousel.component';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MovieRatingComponent } from './components/movie-rating/movie-rating.component';
import { MoviesListComponent } from './movies-list/movies-list.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MovieRatingsComponent } from './components/movie-rating/movie-ratings/movie-ratings.component';

@NgModule({
  declarations: [HoverClassDirective, PaginatorComponent, SidenavResponsiveComponent, TopNavigationBarComponent, MoviesCardComponent, 
              MoviesCarouselComponent, MovieRatingComponent, MoviesListComponent, MovieRatingsComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TranslateModule, MaterialModule, FlexLayoutModule, RouterModule, DragDropModule, MatToolbarModule, 
            ScrollingModule, MatProgressSpinnerModule, MatSnackBarModule],
  exports: [CommonModule, FormsModule, ReactiveFormsModule, TranslateModule, HoverClassDirective, PaginatorComponent, MaterialModule, SidenavResponsiveComponent,
            FlexLayoutModule, DragDropModule, TopNavigationBarComponent, MoviesCardComponent, MoviesCarouselComponent, ScrollingModule, MovieRatingComponent, 
            MatProgressSpinnerModule, MoviesListComponent, MatSnackBarModule, MovieRatingsComponent],
  providers: [SidenavService]
})
export class SharedModule { }