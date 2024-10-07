import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthForgotComponent } from './auth-forgot.component';

describe('AuthForgotComponent', () => {
  let component: AuthForgotComponent;
  let fixture: ComponentFixture<AuthForgotComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthForgotComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthForgotComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
