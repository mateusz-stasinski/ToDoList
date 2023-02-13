import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { TaskPriority, TaskState } from '../models/to-do-task';
import { SnackBarService } from '../services/snack-bar.service';
import { ToDoTasksService } from '../services/to-do-tasks.service';

@Component({
  selector: 'app-calendar-task-dialog',
  templateUrl: './calendar-task-dialog.component.html',
  styleUrls: ['./calendar-task-dialog.component.scss']
})
export class CalendarTaskDialogComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private sb: SnackBarService,
    private translate: TranslateService,
    private tasksServ: ToDoTasksService,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  isLoading:boolean = false;
  showErr:boolean = false;

  states = TaskState;
  priorities = TaskPriority;

  taskForm = this.fb.group({
    day: new FormControl(null, [Validators.required]),
    description: new FormControl(null, [Validators.required]),
    priority: new FormControl(1, [Validators.required]),
    state: new FormControl(0, [Validators.required])
  });


  ngOnInit(): void {

    if (this.data.task) {
      this.taskForm.addControl('id', new FormControl('', [Validators.required]), { emitEvent: false });
      this.taskForm.patchValue(this.data.task, { emitEvent: false });
    } else {
      this.taskForm.get('state')?.disable({ emitEvent: false });

      if (this.data.day) {
        this.taskForm.get('day')?.setValue(this.data.day, { emitEvent: false });
      } else {
        this.taskForm.get('day')?.setValue(new Date, { emitEvent: false });
      }
    }
  }

  save() {
    if(this.taskForm.invalid) {
      this.showErr = true;
      this.sb.open(this.translate.instant('formInvalid'), 'error');
      return;
    }
    if (this.data.task) {
      this.update();
    } else {
      this.add();
    }
  }

  add() {
    this.isLoading = true;
    this.tasksServ.AddTask(this.taskForm.getRawValue()).subscribe({
      next: () => {
        this.tasksServ.refresh();
        this.dialog.closeAll();
      },
      error: (err) => {
        console.log(err)
        if (typeof err?.error === "string" && err?.error != "" && this.translate.instant(err?.error) != err.error) {
          this.sb.open(this.translate.instant(err.error), 'error');
        } else {
          this.sb.open(this.translate.instant('actionFail'), 'error');
        }
      }
    }).add(() => { this.isLoading = false; });
  }

  update() {
    this.isLoading = true;
    this.tasksServ.UpdateTask(this.taskForm.getRawValue()).subscribe({
      next: () => {
        this.tasksServ.refresh();
        this.dialog.closeAll();
      },
      error: (err) => {
        if (typeof err?.error === "string" && err?.error != "" && this.translate.instant(err?.error) != err.error) {
          this.sb.open(this.translate.instant(err.error), 'error');
        } else {
          this.sb.open(this.translate.instant('actionFail'), 'error');
        }
      }
    }).add(() => { this.isLoading = false; });
  }
  
  delete() {
    this.isLoading = true;
    this.tasksServ.DeleteTasks(this.taskForm.get('id')?.value).subscribe({
      next: () => {
        this.tasksServ.refresh();
        this.dialog.closeAll();
      },
      error: (err) => {
        console.log(err)
        if (typeof err?.error === "string" && err?.error != "" && this.translate.instant(err?.error) != err.error) {
          this.sb.open(this.translate.instant(err.error), 'error');
        } else {
          this.sb.open(this.translate.instant('actionFail'), 'error');
        }
      }
    }).add(() => { this.isLoading = false; });
  }

  setMidDay(event: any, control: AbstractControl) {
    const datepipe: DatePipe = new DatePipe('en-US')
    let formattedDate = datepipe.transform(event?.value, 'yyyy-MM-dd')
    formattedDate += 'T12:00:00.000+00:00'
    control.setValue(formattedDate, { emitEvent: false });
  }
}
