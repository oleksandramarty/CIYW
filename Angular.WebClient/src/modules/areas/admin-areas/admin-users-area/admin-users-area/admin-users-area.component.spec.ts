import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUsersAreaComponent } from './admin-users-area.component';

describe('AdminUsersAreaComponent', () => {
  let component: AdminUsersAreaComponent;
  let fixture: ComponentFixture<AdminUsersAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminUsersAreaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminUsersAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
