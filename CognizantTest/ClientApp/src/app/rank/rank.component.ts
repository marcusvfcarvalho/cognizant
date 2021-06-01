import { Component, OnInit } from '@angular/core';
import { TaskService } from '../header/services/task.service';
import { Rank } from '../models/rank';

@Component({
  selector: 'app-rank',
  templateUrl: './rank.component.html',
  styleUrls: ['./rank.component.css']
})
export class RankComponent implements OnInit {
  constructor(
    private taskService: TaskService
  ) { }

  rank: Rank[] = [];

  ngOnInit(): void {
    this.taskService.getTop().subscribe(res => {
      this.rank = res;
    })
  }
}
