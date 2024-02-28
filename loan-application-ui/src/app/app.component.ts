import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { FilterService } from './services/filter.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  showSidenav = false;

  constructor(private router: Router, private filterService: FilterService) {}
  

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        // Adjust the condition based on your results page path
        this.showSidenav = event.urlAfterRedirects.includes('/results');
      }
    });
  }

  onFilterChange(filter: any) {
    console.log('filter in results')
    this.filterService.updateFilter(filter);
  }
}
