import { createReducer, on } from '@ngrx/store';
import * as LoanApplicationActions from '../actions/loan-application.actions';

export const loanApplicationFeatureKey = 'loanApplication';

export interface State {
  // Define your state shape here, e.g., applicationStatus: 'pending' | 'complete' | 'error';
}

export const initialState: State = {
  // Initial state values
};

export const reducer = createReducer(
  initialState,
  on(LoanApplicationActions.submitApplication, state => ({
    ...state,
    applicationStatus: 'pending'
  })),
  // Handle other actions
);
