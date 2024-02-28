import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { HelpDialogComponent } from '../../components/help-dialog/help-dialog.component';

@Component({
  selector: 'personal-details',
  templateUrl: './personal-details.component.html',
  styleUrls: ['./personal-details.component.scss'] 

})
export class PersonalDetailsComponent implements OnInit {
  @Input() formGroup!: FormGroup;
  @Output() formGroupChange = new EventEmitter<FormGroup>();

  titles = ['Mr', 'Mrs', 'Miss', 'Other'];
  maritalStatuses = [
    'Married / Civil partnership',
    'Single',
    'Divorced',
    'Widowed',
    'Other',
  ];

  constructor(private fb: FormBuilder, private dialog: MatDialog) {}

  ngOnInit() {
    this.formGroup = this.fb.group({
      title: ['', Validators.required],
      firstName: ['', [Validators.required, Validators.minLength(5)]],
      lastName: ['', [Validators.required, Validators.minLength(1)]],
      dateOfBirth: ['', Validators.required],
      maritalStatus: ['', Validators.required],
    });

    this.formGroup.valueChanges.subscribe((value) => {
      this.formGroupChange.emit(this.formGroup);
    });
  }

  getErrorMessage(controlName: string): string | null {
    const control = this.formGroup.get(controlName);

    if (!control) return null;

    if (control.hasError('required')) {
      return 'This field is required.';
    } else if (control.hasError('minlength')) {
      return `Minimum length should be ${control.errors?.['minLength'].requiredLength} characters.`;
    }
    return null;
  }

  openDialog() {
    this.dialog.open(HelpDialogComponent, {
      width: '250px',
      data: {
        helpText: 'Some Helptext',
      },
    });
  }
}
