import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminHomeAreaComponent } from './admin-home-area.component';

describe('AdminHomeAreaComponent', () => {
  let component: AdminHomeAreaComponent;
  let fixture: ComponentFixture<AdminHomeAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminHomeAreaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminHomeAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
