import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RankComponent } from './rank/rank.component';
import { TestFormComponent } from './test-form/test-form.component';

const routes: Routes = [
  { path: '', component: TestFormComponent },
  { path: 'test-form', component: TestFormComponent },
  { path: 'top', component: RankComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
