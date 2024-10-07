import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateUserProjectComponent } from './create-update-user-project.component';

describe('CreateUpdateUserProjectComponent', () => {
  let component: CreateUpdateUserProjectComponent;
  let fixture: ComponentFixture<CreateUpdateUserProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUpdateUserProjectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUpdateUserProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
