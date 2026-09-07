import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let userErrorMessage = 'Ocurrió un error inesperado en el sistema.';

      if (error.status === 0) {
        // Error de red, API caída o CORS
        userErrorMessage = 'No se pudo establecer conexión con el servidor. Verifique si la API en http://localhost:5122 está en ejecución.';
      } else if (error.status >= 500) {
        // Excepción interna del servidor (Error 500 / Exception en C#)
        userErrorMessage = error.error?.message || `Error interno del servidor (${error.status}). Por favor, intente más tarde.`;
      } else if (error.status === 400) {
        // Errores de validación enviados por la API
        userErrorMessage = error.error?.message || 'Los datos enviados no cumplen con las reglas del sistema.';
      } else if (error.status === 409) {
        // Conflictos (ej. correo duplicado)
        userErrorMessage = error.error?.message || 'Ya existe un registro con la información ingresada.';
      }

      console.error('Excepción HTTP capturada:', error);
      return throwError(() => new Error(userErrorMessage));
    })
  );
};
