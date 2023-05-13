import { CoreModule } from './modules/core/core.module';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app-component/app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './modules/shared/shared.module';
import { HomeModule } from './modules/home/home.module';
import { MovieModule } from './modules/movie/movie.module';
import { GenresModule } from './modules/genres/genres.module';
import { AdvancedSearchModule } from './modules/advanced-search/advanced-search.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CoreModule,
    SharedModule,
    HomeModule,
    MovieModule,
    GenresModule,
    AdvancedSearchModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
