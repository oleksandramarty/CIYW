import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAuditTrailAreaComponent } from './admin-audit-trail-area.component';

describe('AdminAuditTrailAreaComponent', () => {
  let component: AdminAuditTrailAreaComponent;
  let fixture: ComponentFixture<AdminAuditTrailAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminAuditTrailAreaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminAuditTrailAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
