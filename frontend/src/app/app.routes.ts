import { Routes } from '@angular/router';
import { StudentListComponent } from './core/components/student-list/student-list.component';

export const routes: Routes = [
  { path: '', redirectTo: 'students', pathMatch: 'full' },
  { path: 'students', component: StudentListComponent },
  { path: '**', redirectTo: 'students' }
];
