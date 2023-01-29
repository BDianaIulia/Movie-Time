import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { AdvancedSearchComponent } from './components/home/advanced-search.component';
import { AdvancedSearchRoutingModule } from './advanced-search-routing.module';

@NgModule({
  declarations: [AdvancedSearchComponent],
  imports: [CommonModule, SharedModule, AdvancedSearchRoutingModule],
})
export class AdvancedSearchModule {}
