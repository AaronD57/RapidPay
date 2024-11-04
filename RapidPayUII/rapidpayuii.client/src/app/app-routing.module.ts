import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CardManagementComponent } from './components/card-management/card-management.component';

const routes: Routes = [
  { path: 'card-management', component: CardManagementComponent },
  // other routes...
];

//@NgModule({
//  imports: [RouterModule.forRoot(routes)],
//  exports: [RouterModule]
//})
export class AppRoutingModule { }
