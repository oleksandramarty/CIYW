import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateExpenseComponent } from './create-update-expense.component';

describe('CreateUpdateExpenseComponent', () => {
  let component: CreateUpdateExpenseComponent;
  let fixture: ComponentFixture<CreateUpdateExpenseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdateExpenseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdateExpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
