import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { LoanDetails } from '../models/LoanDetails';
import { PersonalDetails } from '../models/PersonalDetails';

@Injectable({
  providedIn: 'root'
})
export class FormDataService {
  private loanDetailsSource = new BehaviorSubject<LoanDetails | null>(null);
  private personalDetailsSource = new BehaviorSubject<PersonalDetails | null>(null);

  loanDetails = this.loanDetailsSource.asObservable();
  personalDetails = this.personalDetailsSource.asObservable();

  updateLoanDetails(details: LoanDetails) {
    this.loanDetailsSource.next(details);
  }

  updatePersonalDetails(details: PersonalDetails) {
    this.personalDetailsSource.next(details);
  }
}
