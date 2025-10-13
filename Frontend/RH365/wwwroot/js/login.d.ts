interface ILoginForm {
    email: HTMLInputElement;
    password: HTMLInputElement;
    submitButton: HTMLButtonElement;
    form: HTMLFormElement;
}
declare class LoginManager {
    private elements;
    private isSubmitting;
    constructor();
    /**
     * Inicializar elementos del DOM
     */
    private initializeElements;
    /**
     * Adjuntar event listeners
     */
    private attachEventListeners;
    /**
     * Manejar submit del formulario
     */
    private handleSubmit;
    /**
     * Manejar tecla Enter
     */
    private handleEnterKey;
    /**
     * Validar formulario completo
     */
    private validateForm;
    /**
     * Validar email
     */
    private validateEmail;
    /**
     * Validar contraseña
     */
    private validatePassword;
    /**
     * Mostrar error en campo
     */
    private showFieldError;
    /**
     * Limpiar error de campo
     */
    private clearFieldError;
    /**
     * Mostrar mensaje de error general
     */
    private showError;
    /**
     * Mostrar mensaje de éxito
     */
    private showSuccess;
    /**
     * Establecer estado de carga
     */
    private setLoadingState;
}
//# sourceMappingURL=login.d.ts.map