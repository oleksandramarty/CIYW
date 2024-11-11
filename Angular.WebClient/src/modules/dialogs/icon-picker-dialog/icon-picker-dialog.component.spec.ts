import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IconPickerDialogComponent } from './icon-picker-dialog.component';

describe('IconPickerComponent', () => {
  let component: IconPickerDialogComponent;
  let fixture: ComponentFixture<IconPickerDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IconPickerDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IconPickerDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
