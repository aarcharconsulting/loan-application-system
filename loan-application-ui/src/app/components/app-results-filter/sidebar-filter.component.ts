import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-sidebar-filter',
  templateUrl: './sidebar-filter.component.html',
})
export class SidebarFilterComponent {
    @Output() filterChange = new EventEmitter<any>(); 

  filterOptions = [
    { label: 'Option 1', value: 'option1' },
    { label: 'Option 2', value: 'option2' },
    // Add more options as needed
  ];

  emitFilterChange(event: any) { // Again, use a specific type for the event if possible
    this.filterChange.emit(event);
  }
}
