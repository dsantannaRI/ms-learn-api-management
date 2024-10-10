import { Routes } from '@angular/router';
import { CreateUserComponent } from './pages/create-user/create-user.component';
import { ListUsersComponent } from './pages/list-users/list-users.component';

export const routes: Routes = [
  { path: 'create-user', component: CreateUserComponent },
  { path: 'list-users', component: ListUsersComponent },
  { path: '', redirectTo: '/list-users', pathMatch: 'full' }
];
