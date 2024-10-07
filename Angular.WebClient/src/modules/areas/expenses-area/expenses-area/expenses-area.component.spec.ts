import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpensesAreaComponent } from './expenses-area.component';

describe('ExpensesAreaComponent', () => {
  let component: ExpensesAreaComponent;
  let fixture: ComponentFixture<ExpensesAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpensesAreaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExpensesAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
