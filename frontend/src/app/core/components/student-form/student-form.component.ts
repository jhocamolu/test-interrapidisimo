import { Component, EventEmitter, Input, Output, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';

// Rutas relativas corregidas (subir a /core/ y luego ir a models / services)
import { Student } from '../../models/student.model';
import { Subject, AVAILABLE_SUBJECTS } from '../../models/subject.model';
import { StudentService } from '../../services/student.service';

@Component({
  selector: 'app-student-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './student-form.component.html',
  styleUrls: ['./student-form.component.css']
})
export class StudentFormComponent implements OnInit, OnChanges {
  @Input() studentToEdit: Student | null = null;
  @Output() formSubmitted = new EventEmitter<void>();
  @Output() cancelEdit = new EventEmitter<void>();

  studentForm!: FormGroup;
  subjectsList: Subject[] = AVAILABLE_SUBJECTS;
  errorMessage: string | null = null;
  isSubmitting = false;

  constructor(private fb: FormBuilder, private studentService: StudentService) {}

  ngOnInit(): void {
    this.initForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['studentToEdit'] && this.studentForm) {
      if (this.studentToEdit) {
        this.studentForm.patchValue({
          name: this.studentToEdit.name,
          email: this.studentToEdit.email
        });
        this.setSubjectSelection(this.studentToEdit.subjectIds);
      } else {
        this.resetForm();
      }
    }
  }

  private initForm(): void {
    this.studentForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      subjects: this.fb.group({})
    });

    const subjectsGroup = this.studentForm.get('subjects') as FormGroup;
    this.subjectsList.forEach(subj => {
      subjectsGroup.addControl(subj.id, this.fb.control(false));
    });
  }

  private setSubjectSelection(selectedIds: string[]): void {
    const subjectsGroup = this.studentForm.get('subjects') as FormGroup;
    this.subjectsList.forEach(subj => {
      subjectsGroup.get(subj.id)?.setValue(selectedIds.includes(subj.id));
    });
  }

  getSelectedSubjectIds(): string[] {
    const subjectsGroup = this.studentForm.get('subjects') as FormGroup;
    return Object.keys(subjectsGroup.controls).filter(id => subjectsGroup.get(id)?.value === true);
  }

  validateBusinessRules(): string | null {
    const selectedIds = this.getSelectedSubjectIds();

    if (selectedIds.length !== 3) {
      return `Debe seleccionar exactamente 3 materias (Seleccionadas: ${selectedIds.length}).`;
    }

    const selectedSubjects = this.subjectsList.filter(s => selectedIds.includes(s.id));
    const teacherIds = selectedSubjects.map(s => s.teacherId);
    const uniqueTeachers = new Set(teacherIds);

    if (uniqueTeachers.size !== selectedIds.length) {
      return 'No se permite seleccionar materias impartidas por el mismo profesor.';
    }

    return null;
  }

 onSubmit(): void {
  this.errorMessage = null;

  if (this.studentForm.invalid) {
    this.studentForm.markAllAsTouched();
    return;
  }

  const validationError = this.validateBusinessRules();
  if (validationError) {
    this.errorMessage = validationError;
    return;
  }

  const formValues = this.studentForm.value;
  const payload: Student = {
    name: formValues.name,
    email: formValues.email,
    subjectIds: this.getSelectedSubjectIds()
  };

  this.isSubmitting = true;

  if (this.studentToEdit && this.studentToEdit.id) {
    // Actualización (PUT)
    payload.id = this.studentToEdit.id;
    this.studentService.updateStudent(this.studentToEdit.id, payload).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.resetForm();
        this.formSubmitted.emit();
      },
      error: (err: Error) => {
        this.isSubmitting = false;
        // Se informa la excepción formateada al usuario
        this.errorMessage = `Error al guardar cambios: ${err.message}`;
      }
    });
  } else {
    // Creación (POST)
    this.studentService.createStudent(payload).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.resetForm();
        this.formSubmitted.emit();
      },
      error: (err: Error) => {
        this.isSubmitting = false;
        // Se informa la excepción formateada al usuario
        this.errorMessage = `Error al registrar estudiante: ${err.message}`;
      }
    });
  }
}

  resetForm(): void {
    this.studentForm.reset();
    this.errorMessage = null;
  }

  onCancel(): void {
    this.resetForm();
    this.cancelEdit.emit();
  }
}
