import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdatePlannedExpenseDialogComponent } from './create-update-planned-expense-dialog.component';

describe('CreateUpdatePlannedExpenseComponent', () => {
  let component: CreateUpdatePlannedExpenseDialogComponent;
  let fixture: ComponentFixture<CreateUpdatePlannedExpenseDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdatePlannedExpenseDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdatePlannedExpenseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
