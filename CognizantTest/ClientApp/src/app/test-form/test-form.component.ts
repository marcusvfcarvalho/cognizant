import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { TaskService } from '../header/services/task.service';
import { ExecutionStatus } from '../models/execution-status';
import { TestTask } from '../models/test-task';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-test-form',
  templateUrl: './test-form.component.html',
  styleUrls: ['./test-form.component.css']
})
export class TestFormComponent implements OnInit {
  tasks: TestTask[] = [];
  selectedTask: TestTask = { id: 0, name: '', description: '', testInputParameter: '', testOutputParameter: '' };

  editorOptions = { theme: 'vs', language: 'csharp' };
  code: string = "using System;\n"
    + "class Program\n" +
    "{\n" +
    " public static void Main(string[] args)\n" +
    " { \n" +
    "     Console.WriteLine(\"Hello World!\");\n" +
    " } \n" +
    "}\n";

  outputMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private router: Router
  ) {
    this.testForm = this.fb.group({
      name: new FormControl('', [Validators.required]),
      task: new FormControl(),
      code: new FormControl(this.code)
    });

    this.testForm.controls.task.valueChanges.subscribe(t => {
      this.selectedTask = t;
    })

    this.taskSubscription = this.taskService.getTasks().subscribe((res => {
      this.tasks = res;
      if (this.tasks?.length > 0) {
        this.selectedTask = this.tasks[0];
      }
    }))
  }

  testForm: FormGroup;
  taskSubscription: Subscription;

  ngOnInit(): void {
  }

  onSubmit() {
    this.outputMessage = "";
    if (!this.testForm.invalid) {
      this.taskService.postResolution(
        {
          taskId: this.selectedTask.id,
          name: this.testForm.controls.name.value,
          code: this.testForm.controls.code.value
        }
      ).subscribe(res => {
        this.outputMessage = res.output;
        switch (res.status) {
          case ExecutionStatus.Success:
          case ExecutionStatus.ResultFailure:
            setTimeout(() => {
              this.router.navigate(['/top']);
            }, 2000);
        }
      })
    } else {
      this.outputMessage = "Please enter all required information";
    }
  }
}
