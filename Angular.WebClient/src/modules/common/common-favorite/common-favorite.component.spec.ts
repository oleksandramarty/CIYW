import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonFavoriteComponent } from './common-favorite.component';

describe('CommonFavoriteComponent', () => {
  let component: CommonFavoriteComponent;
  let fixture: ComponentFixture<CommonFavoriteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommonFavoriteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommonFavoriteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
