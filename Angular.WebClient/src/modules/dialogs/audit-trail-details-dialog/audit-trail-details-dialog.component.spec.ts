import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditTrailDetailsDialogComponent } from './audit-trail-details-dialog.component';

describe('AuditTrailDetailsDialogComponent', () => {
  let component: AuditTrailDetailsDialogComponent;
  let fixture: ComponentFixture<AuditTrailDetailsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuditTrailDetailsDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuditTrailDetailsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
