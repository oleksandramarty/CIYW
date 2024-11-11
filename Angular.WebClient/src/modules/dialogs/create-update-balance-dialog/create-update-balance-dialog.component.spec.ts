import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateBalanceDialogComponent } from './create-update-balance-dialog.component';

describe('CreateUpdateBalanceComponent', () => {
  let component: CreateUpdateBalanceDialogComponent;
  let fixture: ComponentFixture<CreateUpdateBalanceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdateBalanceDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdateBalanceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
