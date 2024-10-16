import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateFavoriteExpenseComponent } from './create-update-favorite-expense.component';

describe('CreateUpdateFavoriteExpenseComponent', () => {
  let component: CreateUpdateFavoriteExpenseComponent;
  let fixture: ComponentFixture<CreateUpdateFavoriteExpenseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdateFavoriteExpenseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdateFavoriteExpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
