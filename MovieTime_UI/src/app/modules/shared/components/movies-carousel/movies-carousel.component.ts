import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import {Component, Input, OnChanges, SimpleChanges} from '@angular/core';
import { Router } from '@angular/router';
import { Movie } from 'src/app/modules/core/models/movie.model';

@Component({
  selector: 'app-movies-carousel',
  templateUrl: './movies-carousel.component.html',
  styleUrls: ['./movies-carousel.component.scss']
})
export class MoviesCarouselComponent implements OnChanges{

  @Input() overviewList: Array<Movie> | undefined;

  public moviesOverviewList: Array<Movie> | undefined;
  public leftPos = 0;

  private cardsNumber = 1;

  private readonly CARDS_NUMBER_XL = 6;
  private readonly CARDS_NUMBER_M = 4;
  private readonly CARDS_NUMBER_S = 3;
  private readonly CARDS_NUMBER_XS = 2;

  constructor(private router: Router, private breakpointObserver: BreakpointObserver) {}

  public ngOnInit(): void {
    this.breakpointObserver.observe([Breakpoints.XLarge, Breakpoints.Large])
    .subscribe((state: BreakpointState) => {
      if (state.matches) {
        this.cardsNumber = this.CARDS_NUMBER_XL;
        this.init();
      }
    });
    this.breakpointObserver.observe([Breakpoints.Medium])
    .subscribe((state: BreakpointState) => {
      if (state.matches) {
        this.cardsNumber = this.CARDS_NUMBER_M;
        this.init();
      }
    });
    this.breakpointObserver.observe([Breakpoints.Small])
    .subscribe((state: BreakpointState) => {
      if (state.matches) {
        this.cardsNumber = this.CARDS_NUMBER_S;
        this.init();
      }
    });
    this.breakpointObserver.observe([Breakpoints.XSmall])
    .subscribe((state: BreakpointState) => {
      if (state.matches) {
        this.cardsNumber = this.CARDS_NUMBER_XS;
        this.init();
      }
    });
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.overviewList && changes.overviewList.currentValue && this.overviewList) {
      this.init();
    }
  }

  public scrollRight(): void {
    if (!this.overviewList || this.leftPos === (this.overviewList.length - (this.cardsNumber - 1))) return;
    this.leftPos++;
    this.moviesOverviewList = this.overviewList?.slice(this.leftPos, this.leftPos + this.cardsNumber);
  }

  public scrollLeft(): void {
    if (this.leftPos === 0) return;
    this.leftPos--;
    this.moviesOverviewList = this.overviewList?.slice(this.leftPos, this.leftPos + this.cardsNumber);
  }

  public onMovieClicked(movie: Movie): void {
    this.router.navigateByUrl(`/movie/${movie.id}`);
  }

  private init() {
    if (!this.overviewList) return;
    
    this.leftPos = 0;
    this.moviesOverviewList = this.overviewList.slice(0, this.cardsNumber);
  }
}
