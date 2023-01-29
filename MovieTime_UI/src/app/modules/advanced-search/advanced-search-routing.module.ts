import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdvancedSearchComponent } from './components/home/advanced-search.component';

const routes: Routes = [
  { path: ':keywords', component: AdvancedSearchComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdvancedSearchRoutingModule {}
