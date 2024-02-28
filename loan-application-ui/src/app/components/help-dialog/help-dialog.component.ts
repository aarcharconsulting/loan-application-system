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
  templateUrl: './help-dialog.component.html'
})
export class HelpDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    console.log('data - ', this.data)
  }
}
