import { Component, OnInit } from '@angular/core';
import { FilterService } from '../../services/filter.service';

@Component({
  selector: 'app-results',
  templateUrl: './app-results.component.html',
})
export class AppResultsComponent implements OnInit {
  results: any[] = []; // Your initial results here
  filteredResults: any[] = [];

  constructor(private filterService: FilterService) {}

  ngOnInit() {
    this.filterService.getFilterState().subscribe(filters => {

      console.log('filter subscribed')

      this.filteredResults = this.filterService.applyFilters(this.results, filters);
    });
  }


}
