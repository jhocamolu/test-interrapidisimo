export interface Student {
  id?: string;
  name: string;
  email: string;
  subjectIds: string[];
}

export interface ClassmateResponse {
  subjectId: string;
  subjectName: string;
  classmates: string[];
}
