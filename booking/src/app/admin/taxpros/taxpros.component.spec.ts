import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaxprosComponent } from './taxpros.component';

describe('TaxprosComponent', () => {
  let component: TaxprosComponent;
  let fixture: ComponentFixture<TaxprosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaxprosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaxprosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
