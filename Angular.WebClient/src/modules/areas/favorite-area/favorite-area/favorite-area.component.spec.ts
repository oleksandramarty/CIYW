import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavoriteAreaComponent } from './favorite-area.component';

describe('FavoriteAreaComponent', () => {
  let component: FavoriteAreaComponent;
  let fixture: ComponentFixture<FavoriteAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FavoriteAreaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FavoriteAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
