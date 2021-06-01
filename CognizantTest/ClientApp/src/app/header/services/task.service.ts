import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { ExecutionResult } from 'src/app/models/execution-result';
import { Rank } from 'src/app/models/rank';
import { TaskSolution } from 'src/app/models/task-solution';
import { TestTask } from 'src/app/models/test-task';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  getTop() {
    return this.http.get<Rank[]>('/api/tasks/top');
  }

  constructor(private http: HttpClient) { }

  getTasks() {
    return this.http.get<TestTask[]>('/api/tasks');
  }

  postResolution(taskSolution: TaskSolution) {
    return this.http.post<ExecutionResult>('/api/tasks/execute', taskSolution);
  }
}
