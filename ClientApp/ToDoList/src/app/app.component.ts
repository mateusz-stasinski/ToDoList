import { DatePipe } from '@angular/common';
import { Component, Injectable, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, Validators } from '@angular/forms';
import { DateAdapter, NativeDateAdapter } from '@angular/material/core';
import { DateRange, DefaultMatCalendarRangeStrategy, MatRangeDateSelectionModel, MAT_DATE_RANGE_SELECTION_STRATEGY } from '@angular/material/datepicker';
import { MatDialog } from '@angular/material/dialog';
import { debounceTime, distinctUntilChanged, Subscription } from 'rxjs';
import { CalendarTaskDialogComponent } from './components/calendar-task-dialog.component';
import { TaskPriority, TasksPerDay, TaskState, ToDoTask } from './models/to-do-task';
import { ToDoTasksService } from './services/to-do-tasks.service';

@Injectable({
  providedIn: 'root'
})
export class CustomDateAdapter extends NativeDateAdapter {
  override parse(value: any): Date | null {
    if (typeof value === 'string' && value.indexOf('/') > -1) {
      const str = value.split('/');

      const year = Number(str[2]);
      const month = Number(str[1]) - 1;
      const date = Number(str[0]);

      return new Date(year, month, date);
    }
    const timestamp = typeof value === 'number' ? value : Date.parse(value);
    return isNaN(timestamp) ? null : new Date(timestamp);
  }

  override getFirstDayOfWeek(): number {
    return 1;
  }
}
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [
    { provide: MAT_DATE_RANGE_SELECTION_STRATEGY, useClass: DefaultMatCalendarRangeStrategy },
    DefaultMatCalendarRangeStrategy,
    MatRangeDateSelectionModel,
    { provide: DateAdapter, useClass: CustomDateAdapter }
  ]
})
export class AppComponent implements OnInit, OnDestroy {

  constructor(
    private fb: FormBuilder,
    private readonly dateRangeSelectionModel: MatRangeDateSelectionModel<Date>,
    private readonly dateRangeSelectionStrategy: DefaultMatCalendarRangeStrategy<Date>,
    private tasksServ: ToDoTasksService,
    private dialog: MatDialog
  ) { }

  isLoading: boolean = true;

  states = TaskState;
  priorities = TaskPriority;

  searchForm = this.fb.group({
    from: new FormControl(null, [Validators.required]),
    to: new FormControl(null, [Validators.required]),
    states: new FormControl([0, 1, 2], [Validators.required]),
    priorities: new FormControl([0, 1, 2], [Validators.required]),
  });

  searchFormDefault = {
    states: [0, 1, 2],
    priorities: [0, 1, 2],
  };

  selectedDateRange?: DateRange<Date>;

  tasksPerDay: TasksPerDay[] = [];
  selectedDays: Date[] = [];

  searchSubscription!: Subscription;
  refreshTaskSubscription!: Subscription;

  ngOnInit(): void {
    this.initStartData();
    this.setSearchFormDateRange();
    this.getTasks();

    this.searchSubscription = this.searchForm.valueChanges.pipe(
      distinctUntilChanged(),
      debounceTime(500)
    ).subscribe(() => { this.search(); });

    this.refreshTaskSubscription = this.tasksServ.refreshingObjSubject.subscribe(() => {
      this.getTasks();
    });
  }

  initStartData() {
    const startDay = new Date();
    const endDay = new Date();
    const day = startDay.getDay();
    startDay.setDate(startDay.getDate() - day + 1)
    endDay.setDate(endDay.getDate() - day + 7);
    this.selectedDateRange = new DateRange(startDay, endDay);
  }

  getTasks() {
    if (this.searchForm.invalid) {
      return;
    }
    this.isLoading = true;
    this.tasksServ.GetTasks(this.searchForm?.value).subscribe({
      next: (result) => {
        this.tasksPerDay = [];
        this.tasksPerDay.push(...result);
      },
      error: (err) => { }
    }).add(() => this.isLoading = false);

  }

  search(): void {
    this.getTasks();
  }

  reset() {
    this.initStartData();
    this.searchForm.reset(
      {
        ...this.searchFormDefault,
        from: this.selectedDateRange?.start,
        to: this.selectedDateRange?.end
      },
      { emitEvent: false }
    );
  }

  setDateRange(selectedDate: any) {
    const selection = this.dateRangeSelectionModel.selection, newSelection = this.dateRangeSelectionStrategy.selectionFinished(
      selectedDate,
      selection
    );
    this.dateRangeSelectionModel.updateSelection(newSelection, this);
    this.selectedDateRange = new DateRange<Date>(newSelection.start, newSelection.end);
    this.setSearchFormDateRange();
  }

  private setSearchFormDateRange() {
    const pipe = new DatePipe('en-US');
    let from = pipe.transform(this.selectedDateRange?.start, 'yyyy-MM-dd') + 'T12:00:00.000+00:00';
    let to = pipe.transform(this.selectedDateRange?.end ?? this.selectedDateRange?.start, 'yyyy-MM-dd') + 'T12:00:00.000+00:00';

    this.searchForm.get('from')?.setValue(from, { emitEvent: false });
    this.searchForm.get('to')?.setValue(to, { emitEvent: false });
  }

  toggleChip(key: number, control: AbstractControl) {
    let arr = [...control?.value];
    if (control?.value.includes(key)) {
      arr = control?.value.filter((x: number) => x != key);
    } else {
      arr.push(key)
    }
    control.setValue(arr);

    if (control?.value?.length === 0 || !control?.value) {
      control.setValue([0, 1, 2].filter((x: number) => x != key))
    }
  }

  openTask(task: ToDoTask | null = null, day: Date | null = null) {
    this.dialog.open(CalendarTaskDialogComponent, {
      data: {
        task: task,
        day: day,
      },
      width: '450px',
      maxHeight: '95vh'
    })
  }

  nextPeroid() {
    const startDay = this.selectedDateRange?.start!
    const endDay = this.selectedDateRange?.end!
    const days = endDay?.getDate() - startDay?.getDate();
    startDay.setDate(startDay.getDate() + 1 + days)
    endDay.setDate(endDay.getDate() + 1 + days);
    this.selectedDateRange = new DateRange(startDay, endDay);
    this.setSearchFormDateRange();
  }

  previousPeroid() {
    const startDay = this.selectedDateRange?.start!
    const endDay = this.selectedDateRange?.end!
    const days = endDay?.getDate() - startDay?.getDate();
    startDay.setDate(startDay.getDate() - 1 - days)
    endDay.setDate(endDay.getDate() - 1 - days);
    this.selectedDateRange = new DateRange(startDay, endDay);
    this.setSearchFormDateRange();
  }

  setMidDay(event: any, control: AbstractControl) {
    const datepipe: DatePipe = new DatePipe('en-US')
    let formattedDate = datepipe.transform(event?.value, 'yyyy-MM-dd')
    formattedDate += 'T12:00:00.000+00:00'
    control.setValue(formattedDate, { emitEvent: false });
  }

  ngOnDestroy(): void {
    this.searchSubscription.unsubscribe();
    this.refreshTaskSubscription.unsubscribe();
  }
}
