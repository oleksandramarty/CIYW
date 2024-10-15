import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonTopMenuComponent } from './common-top-menu.component';

describe('CommonTopMenuComponent', () => {
  let component: CommonTopMenuComponent;
  let fixture: ComponentFixture<CommonTopMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommonTopMenuComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommonTopMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
