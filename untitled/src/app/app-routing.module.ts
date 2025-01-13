import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RegisterComponent} from "./register/register.component";
import {HomepageComponent} from "./homepage/homepage.component";
import {QuestionpageComponent} from "./questionpage/questionpage.component";
import {AddQuestionComponent} from "./add-question/add-question.component";
import {UserPageComponent} from "./user-page/user-page.component";

const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'homepage', component: HomepageComponent },
  { path: 'question/:id', component: QuestionpageComponent }, // Question page with questionId
  { path: 'add-question', component: AddQuestionComponent }, // Add Question route
  { path: 'user', component: UserPageComponent },
];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
