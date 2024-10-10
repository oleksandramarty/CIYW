import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardUserProjectsComponent } from './dashboard-user-projects.component';

describe('ExpenseDashboardProjectsComponent', () => {
  let component: DashboardUserProjectsComponent;
  let fixture: ComponentFixture<DashboardUserProjectsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardUserProjectsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardUserProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
