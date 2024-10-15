import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProjectPlannedExpensesComponent } from './user-project-planned-expenses.component';

describe('UserProjectPlannedExpensesComponent', () => {
  let component: UserProjectPlannedExpensesComponent;
  let fixture: ComponentFixture<UserProjectPlannedExpensesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserProjectPlannedExpensesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserProjectPlannedExpensesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
