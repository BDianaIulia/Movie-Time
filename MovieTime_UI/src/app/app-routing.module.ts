import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app-component/app.component';
import { AuthGuard } from './modules/core/guards/auth.guard';
import { LoginGuard } from './modules/core/guards/login.guard';
import { HomeComponent } from './modules/home/components/home/home.component';

const routes: Routes = [
      {
        path: 'home',
        loadChildren: () =>
          import('./modules/home/home.module').then((m) => m.HomeModule),
        //canActivate: [AuthGuard],
      },
      {
        path: 'movie',
        loadChildren: () =>
          import('./modules/movie/movie.module').then((m) => m.MovieModule),
        //canActivate: [AuthGuard],
      },
      {
        path: 'genres',
        loadChildren: () =>
          import('./modules/genres/genres.module').then((m) => m.GenresModule),
        //canActivate: [AuthGuard],
      },
      {
        path: 'advanced-search',
        loadChildren: () =>
          import('./modules/advanced-search/advanced-search.module').then((m) => m.AdvancedSearchModule),
        //canActivate: [AuthGuard],
      },
      {
        path: 'account',
        loadChildren: () =>
          import('./modules/account/account.module').then((m) => m.AccountModule),
        canActivate: [LoginGuard],
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'home'
      }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
