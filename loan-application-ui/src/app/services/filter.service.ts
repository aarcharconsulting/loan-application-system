import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FilterService {
  private filterState = new BehaviorSubject<any>({});

  updateFilter(filter: any) {
    this.filterState.next(filter);
  }

  getFilterState() {
    return this.filterState.asObservable();
  }

  // Method to apply filters to the data
  // In FilterService
  applyFilters(results: any[], filters: any): any[] {
    // Implement filtering logic based on filters and return filtered results
    // This is a very basic example; you'll need to adjust it based on your actual filters and data structure
    return results.filter((result) => {
      // Example: return result matches filters criteria
      return filters ? result.filterCriteria === filters.value : true;
    });
  }
}
