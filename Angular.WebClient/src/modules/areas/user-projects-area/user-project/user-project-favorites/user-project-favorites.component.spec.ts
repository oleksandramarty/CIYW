import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserProjectFavoritesComponent } from './user-project-favorites.component';

describe('UserProjectFavoritesComponent', () => {
  let component: UserProjectFavoritesComponent;
  let fixture: ComponentFixture<UserProjectFavoritesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserProjectFavoritesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserProjectFavoritesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
