import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProjectExpensesComponent } from './user-project-expenses.component';

describe('UserProjectExpensesComponent', () => {
  let component: UserProjectExpensesComponent;
  let fixture: ComponentFixture<UserProjectExpensesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserProjectExpensesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserProjectExpensesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
