import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AppComponent } from './app.component';
import { appRoutes } from './app.routes';
import { LoanDetailsComponent } from './pages/loan-details/loan-details.component';
import { PersonalDetailsComponent } from './pages/personal-details/personal-details.component';
import { AddressDetailsComponent } from './pages/address-details/address-details.component';
import { YourFinancesComponent } from './pages/your-finances/your-finances.component';
import { AppStepperComponent } from './components/app-stepper/app-stepper.component';
import { SidebarFilterComponent } from './components/app-results-filter/sidebar-filter.component';
import { AppResultsComponent } from './components/app-results/app-results.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatStepperModule } from '@angular/material/stepper';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    AppStepperComponent,
    LoanDetailsComponent,
    PersonalDetailsComponent,
    AddressDetailsComponent,
    YourFinancesComponent,
    SidebarFilterComponent,
    AppResultsComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(appRoutes),
    StoreModule.forRoot({}, {}),
    EffectsModule.forRoot([]),
    MatSidenavModule,
    MatToolbarModule,
    MatStepperModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatRadioModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    CommonModule
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' } // For Datepicker locale if needed
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
