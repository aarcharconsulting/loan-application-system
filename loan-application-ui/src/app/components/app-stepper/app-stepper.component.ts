import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule,
  FormGroup,
} from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatStepperModule } from '@angular/material/stepper';
import { MatButtonModule } from '@angular/material/button';
import { LoanDetails } from '../../models/LoanDetails';
import { Router } from '@angular/router';

@Component({
  selector: 'app-stepper.component',
  templateUrl: './app-stepper.component.html',
})
export class AppStepperComponent implements OnInit {
  loanDetails: LoanDetails = { amount: null, years: null };
  years = Array.from({ length: 25 }, (_, i) => i);
  employmentList = [{'text':'Full time', 'value':'1'},{'text':'Self employed', 'value':'2'},{'text':'Part time', 'value':'3'},{'text':'Retired', 'value':'4'},{'text':'Unemployed', 'value':'5'}]
  loanDetailsFormGroup = this._formBuilder.group({
    amountCtrl: [''],
    yearCtrl: [''],
  });
  personalDetailsFormGroup = this._formBuilder.group({});
  addressFormGroup = this._formBuilder.group({
    addressCtrl: [''],
    noOfYearsCtrl: ['']
  });

  financesFormGroup = this._formBuilder.group({
    employmentStatus: [''],
    annualIncomeCtrl: ['']
  });
  isLinear = false;

  constructor(private _formBuilder: FormBuilder,private router: Router) {}

  ngOnInit() {}

  navigateToResults() {
    this.router.navigate(['/results']);
  }

  onPersonalDetailsFormChange(updatedFormGroup: FormGroup) {
    this.personalDetailsFormGroup = updatedFormGroup;
    debugger;
  }
}
