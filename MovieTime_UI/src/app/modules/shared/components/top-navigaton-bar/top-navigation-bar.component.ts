import {Component, EventEmitter, HostListener, Output} from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-top-navigation-bar',
  templateUrl: './top-navigation-bar.component.html',
  styleUrls: ['./top-navigation-bar.component.scss']
})
export class TopNavigationBarComponent {

  public isScrolled = false;
  public searchedValue = "";

  @HostListener('window:scroll')
  scrollEvent() {
    this.isScrolled = window.scrollY >= 30;
  }

  constructor(private router: Router) {}

  public onSearch() {
    if (this.searchedValue === '') return;
    this.router.navigateByUrl(`/advanced-search/${this.searchedValue}`);
    this.searchedValue = '';
  }
}
