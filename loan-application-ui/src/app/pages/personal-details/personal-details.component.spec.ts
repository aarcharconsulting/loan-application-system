import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PersonalDetailsComponent } from './personal-details.component';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';

describe('PersonalDetailsComponent', () => {
  let component: PersonalDetailsComponent;
  let fixture: ComponentFixture<PersonalDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonalDetailsComponent ],
      imports: [ ReactiveFormsModule ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(PersonalDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('form invalid when empty', () => {
    expect(component.formGroup.valid).toBeFalsy();
  });

  it('first name field validity', () => {
    let firstName = component.formGroup.controls['firstName'];
    expect(firstName.valid).toBeFalsy();
    firstName.setValue("John");
    expect(firstName.valid).toBeTruthy();
  });

});
