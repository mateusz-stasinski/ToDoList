import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarTaskDialogComponent } from './calendar-task-dialog.component';

describe('CalendarTaskDialogComponent', () => {
  let component: CalendarTaskDialogComponent;
  let fixture: ComponentFixture<CalendarTaskDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarTaskDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarTaskDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
