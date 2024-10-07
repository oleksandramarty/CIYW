import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpenseDashboardProjectsComponent } from './expense-dashboard-projects.component';

describe('ExpenseDashboardProjectsComponent', () => {
  let component: ExpenseDashboardProjectsComponent;
  let fixture: ComponentFixture<ExpenseDashboardProjectsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExpenseDashboardProjectsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExpenseDashboardProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
