import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RegisterComponent} from "./register/register.component";
import {HomepageComponent} from "./homepage/homepage.component";

const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'homepage', component: HomepageComponent },
];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
