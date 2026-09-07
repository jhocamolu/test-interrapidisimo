import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Student, ClassmateResponse } from '../models/student.model';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  // Asegúrate de que coincida con el puerto donde corre dotnet (HTTP: 5122 / HTTPS: 7189)
  private readonly baseUrl = 'https://localhost:7189/api/students';

  constructor(private http: HttpClient) {}

  getStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(this.baseUrl);
  }

  createStudent(student: Student): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.baseUrl, student);
  }

  updateStudent(id: string, student: Student): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, student);
  }

  deleteStudent(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getClassmates(studentId: string): Observable<ClassmateResponse[]> {
    return this.http.get<ClassmateResponse[]>(`${this.baseUrl}/${studentId}/classmates`);
  }
}
