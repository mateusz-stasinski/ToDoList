import { Pipe, PipeTransform } from "@angular/core";

export interface ToDoTask {
  id: string;
	day: string;
	description: string;
  state: number;
  priority: number;
}

export interface TaskAddRequest {
	day: string;
	description: string;
  priority: number;
}

export interface TaskUpdateRequest {
  id: string;
	day: string;
	description: string;
  state: number;
  priority: number;
}

export interface TasksPerDay {
  day: Date;
  dayName: string;
  tasks: ToDoTask[];
}

export const TaskState: IDictionary[] = [
  { key: 0, value: "toDo" },
  { key: 1, value: "doing" },
  { key: 2, value: "done" },
]

export const TaskPriority: IDictionary[] = [
  { key: 0, value: "low" },
  { key: 1, value: "medium" },
  { key: 2, value: "height" },
]

export interface IDictionary {
  key: number,
  value: string
}

@Pipe({name: 'dictionaryKey'})
export class DictionaryPipe implements PipeTransform {
  transform(key: any, types: IDictionary[]): string {
    return types.find(t => t.key == key)?.value ?? '';
  }
}