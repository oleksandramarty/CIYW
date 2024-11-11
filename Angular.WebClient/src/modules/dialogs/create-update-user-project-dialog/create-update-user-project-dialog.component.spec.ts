import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateUserProjectDialogComponent } from './create-update-user-project-dialog.component';

describe('CreateUpdateUserProjectComponent', () => {
  let component: CreateUpdateUserProjectDialogComponent;
  let fixture: ComponentFixture<CreateUpdateUserProjectDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdateUserProjectDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdateUserProjectDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
