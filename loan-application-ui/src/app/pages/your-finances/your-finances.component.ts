import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-your-finances',
  templateUrl: './your-finances.component.html',
})
export class YourFinancesComponent {
  constructor(private router: Router) {}

  navigateToResults() {
    this.router.navigate(['/results']);
  }
}
