import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InputIconPickerComponent } from './input-icon-picker.component';

describe('InputIconPickerComponent', () => {
  let component: InputIconPickerComponent;
  let fixture: ComponentFixture<InputIconPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InputIconPickerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InputIconPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
