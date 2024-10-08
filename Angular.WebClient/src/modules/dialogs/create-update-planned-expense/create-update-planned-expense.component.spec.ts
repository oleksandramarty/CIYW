import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdatePlannedExpenseComponent } from './create-update-planned-expense.component';

describe('CreateUpdatePlannedExpenseComponent', () => {
  let component: CreateUpdatePlannedExpenseComponent;
  let fixture: ComponentFixture<CreateUpdatePlannedExpenseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdatePlannedExpenseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdatePlannedExpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
