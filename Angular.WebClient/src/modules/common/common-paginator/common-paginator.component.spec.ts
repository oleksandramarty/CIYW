import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonPaginatorComponent } from './common-paginator.component';

describe('CommonPaginatorComponent', () => {
  let component: CommonPaginatorComponent;
  let fixture: ComponentFixture<CommonPaginatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommonPaginatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommonPaginatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
