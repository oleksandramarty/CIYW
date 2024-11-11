import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateExpenseDialogComponent } from './create-update-expense-dialog.component';

describe('CreateUpdateExpenseComponent', () => {
  let component: CreateUpdateExpenseDialogComponent;
  let fixture: ComponentFixture<CreateUpdateExpenseDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdateExpenseDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdateExpenseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
