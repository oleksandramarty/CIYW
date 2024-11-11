import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateFavoriteExpenseDialogComponent } from './create-update-favorite-expense-dialog.component';

describe('CreateUpdateFavoriteExpenseComponent', () => {
  let component: CreateUpdateFavoriteExpenseDialogComponent;
  let fixture: ComponentFixture<CreateUpdateFavoriteExpenseDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdateFavoriteExpenseDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdateFavoriteExpenseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
