import { Routes } from '@angular/router';
import { AppStepperComponent } from './components/app-stepper/app-stepper.component';
import { AppResultsComponent } from './components/app-results/app-results.component';

export const appRoutes: Routes = [
  { path: '', component: AppStepperComponent },
  { path: 'results', component: AppResultsComponent },

];
