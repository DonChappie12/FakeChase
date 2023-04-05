import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

//* Components */
import { GeneralDashboardComponent } from './components/general-dashboard/general-dashboard.component';
import { AdminComponent } from './components/admin/admin.component';
import { RegisterComponent } from './components/register/register.component';

const routes: Routes = [
  {path: "", pathMatch: "full", component: GeneralDashboardComponent},
  {path: "admin", component: AdminComponent},
  {path: "register", component: RegisterComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
