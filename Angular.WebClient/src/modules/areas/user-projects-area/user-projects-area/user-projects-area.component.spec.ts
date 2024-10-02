import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProjectsAreaComponent } from './user-projects-area.component';

describe('UserProjectsAreaComponent', () => {
  let component: UserProjectsAreaComponent;
  let fixture: ComponentFixture<UserProjectsAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserProjectsAreaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserProjectsAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
