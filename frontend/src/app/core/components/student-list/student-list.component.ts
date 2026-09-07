import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';

// Rutas relativas corregidas
import { StudentService } from '../../services/student.service';
import { Student, ClassmateResponse } from '../../models/student.model';
import { StudentFormComponent } from '../student-form/student-form.component';

@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [CommonModule, StudentFormComponent],
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.css']
})
export class StudentListComponent implements OnInit {
  students: Student[] = [];
  selectedStudentToEdit: Student | null = null;
  selectedClassmates: ClassmateResponse[] | null = null;
  selectedStudentName: string = '';

  loading = false;
  apiError: string | null = null;
  isModalOpen = false;

  constructor(private studentService: StudentService) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.loading = true;
    this.apiError = null;
    this.studentService.getStudents().subscribe({
      next: (data: Student[]) => {
        this.students = data;
        this.loading = false;
      },
      error: (err: HttpErrorResponse) => {
        this.apiError = 'No se pudo conectar con la API (http://localhost:5122). Verifique la conexión.';
        this.loading = false;
      }
    });
  }

  onEdit(student: Student): void {
    this.selectedStudentToEdit = { ...student };
  }

  onDelete(id: string): void {
    if (confirm('¿Está seguro de eliminar este estudiante?')) {
      this.studentService.deleteStudent(id).subscribe({
        next: () => this.loadStudents(),
        error: (err: HttpErrorResponse) => alert('Error al eliminar el estudiante.')
      });
    }
  }

  onViewClassmates(student: Student): void {
    if (!student.id) return;
    this.selectedStudentName = student.name;
    this.studentService.getClassmates(student.id).subscribe({
      next: (data: ClassmateResponse[]) => {
        this.selectedClassmates = data;
        this.isModalOpen = true;
      },
      error: (err: HttpErrorResponse) => alert('Error al obtener la lista de compañeros.')
    });
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.selectedClassmates = null;
  }

  onFormSubmitted(): void {
    this.selectedStudentToEdit = null;
    this.loadStudents();
  }

  onCancelEdit(): void {
    this.selectedStudentToEdit = null;
  }
}
