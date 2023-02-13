import { TestBed } from '@angular/core/testing';

import { ToDoTasksService } from './to-do-tasks.service';

describe('ToDoTasksService', () => {
  let service: ToDoTasksService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ToDoTasksService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
