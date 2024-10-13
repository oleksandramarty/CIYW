import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateBalanceComponent } from './create-update-balance.component';

describe('CreateUpdateBalanceComponent', () => {
  let component: CreateUpdateBalanceComponent;
  let fixture: ComponentFixture<CreateUpdateBalanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdateBalanceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdateBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
