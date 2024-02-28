import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogTitle,
} from '@angular/material/dialog';

import { MatButtonModule } from '@angular/material/button';
@Component({
  selector: 'help-dialog',
  standalone: true,
  imports: [
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    MatButtonModule,
  ],
  template: `<h2 mat-dialog-title>Help</h2>
  <mat-dialog-content>{{ data.helpText }}</mat-dialog-content>
  <mat-dialog-actions>
      <button mat-button mat-dialog-close>Close</button>
  </mat-dialog-actions>`,
})
export class HelpDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {}
}
