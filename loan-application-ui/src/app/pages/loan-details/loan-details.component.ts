// LoanDetailsComponent.ts
import { Component, OnInit } from '@angular/core';
import { FormDataService } from '../../services/form-data-service';
import { LoanDetails } from '../../models/LoanDetails';

@Component({
  selector: 'app-loan-details',
  templateUrl: './loan-details.component.html',
  
})
export class LoanDetailsComponent implements OnInit {
  loanDetails: LoanDetails = { amount: null, years: null };
  years = Array.from({length: 25}, (_, i) => i + 1);

  constructor(private formDataService: FormDataService) {}

  ngOnInit() {
    this.formDataService.loanDetails.subscribe(details => {
      if (details) this.loanDetails = details;
    });
  }
 
}
