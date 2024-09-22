import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthRestoreComponent } from './auth-restore.component';

describe('AuthResetComponent', () => {
  let component: AuthRestoreComponent;
  let fixture: ComponentFixture<AuthRestoreComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthRestoreComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthRestoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
