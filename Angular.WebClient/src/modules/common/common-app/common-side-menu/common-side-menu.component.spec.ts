import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonSideMenuComponent } from './common-side-menu.component';

describe('CommonSideMenuComponent', () => {
  let component: CommonSideMenuComponent;
  let fixture: ComponentFixture<CommonSideMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommonSideMenuComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommonSideMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
