import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonQuickActionsComponent } from './common-quick-actions.component';

describe('CommonQuickActionsComponent', () => {
  let component: CommonQuickActionsComponent;
  let fixture: ComponentFixture<CommonQuickActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommonQuickActionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommonQuickActionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
