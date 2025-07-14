import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookingConfirmationsComponent } from './booking-confirmations.component';

describe('BookingConfirmationsComponent', () => {
  let component: BookingConfirmationsComponent;
  let fixture: ComponentFixture<BookingConfirmationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookingConfirmationsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookingConfirmationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
