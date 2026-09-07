export interface Subject {
  id: string;
  name: string;
  credits: number;
  teacherId: string;
  teacherName?: string;
}

export const AVAILABLE_SUBJECTS: Subject[] = [
  { id: '01000000-0000-0000-0000-000000000001', name: 'Matemáticas I', credits: 3, teacherId: 'PROF-01' },
  { id: '01000000-0000-0000-0000-000000000002', name: 'Álgebra Lineal', credits: 3, teacherId: 'PROF-01' },
  { id: '02000000-0000-0000-0000-000000000001', name: 'Física General', credits: 3, teacherId: 'PROF-02' },
  { id: '02000000-0000-0000-0000-000000000002', name: 'Mecánica', credits: 3, teacherId: 'PROF-02' },
  { id: '03000000-0000-0000-0000-000000000001', name: 'Programación I', credits: 3, teacherId: 'PROF-03' },
  { id: '03000000-0000-0000-0000-000000000002', name: 'Bases de Datos', credits: 3, teacherId: 'PROF-03' },
  { id: '04000000-0000-0000-0000-000000000001', name: 'Estructura de Datos', credits: 3, teacherId: 'PROF-04' },
  { id: '04000000-0000-0000-0000-000000000002', name: 'Redes de Computadores', credits: 3, teacherId: 'PROF-04' },
  { id: '05000000-0000-0000-0000-000000000001', name: 'Sistemas Operativos', credits: 3, teacherId: 'PROF-05' },
  { id: '05000000-0000-0000-0000-000000000002', name: 'Ingeniería de Software', credits: 3, teacherId: 'PROF-05' }
];
