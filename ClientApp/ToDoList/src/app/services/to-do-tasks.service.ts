import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TaskAddRequest, TasksPerDay, TaskUpdateRequest, ToDoTask } from '../models/to-do-task';

@Injectable({
  providedIn: 'root'
})
export class ToDoTasksService {

  constructor(
    private http: HttpClient,
  ) { }

  public refreshingObjSubject = new Subject<void>();
  public refresh() {
    this.refreshingObjSubject.next();
  }

  public AddTask(request: TaskAddRequest): Observable<ToDoTask> {
    return this.http.post<ToDoTask>(`${environment.apiUrl}/ToDoTask/add`, request);
  }

  public UpdateTask(request: TaskUpdateRequest): Observable<ToDoTask> {
    return this.http.put<ToDoTask>(`${environment.apiUrl}/ToDoTask/update`, request);
  }

  public GetTasks(request: any): Observable<TasksPerDay[]> {
    return this.http.post<TasksPerDay[]>(`${environment.apiUrl}/ToDoTask/find`, request);
  }

  public DeleteTasks(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/ToDoTask/delete/${id}`);
  }
}
