"use strict";
// ============================================================================
// Archivo: login.ts
// Proyecto: RH365.WebMVC
// Ruta: RH365/TS/login.ts
// Descripción:
//   - Script TypeScript para manejo del formulario de login
//   - Validaciones client-side y llamadas AJAX al controller
// ============================================================================
class LoginManager {
    constructor() {
        this.isSubmitting = false;
        this.initializeElements();
        if (this.elements) {
            this.attachEventListeners();
        }
    }
    /**
     * Inicializar elementos del DOM
     */
    initializeElements() {
        const email = document.getElementById('email');
        const password = document.getElementById('password');
        const submitButton = document.querySelector('button[type="submit"]');
        const form = document.querySelector('form');
        // Verificar que todos los elementos existen
        if (!email || !password || !submitButton || !form) {
            console.error('No se encontraron todos los elementos del formulario');
            return;
        }
        this.elements = {
            email: email,
            password: password,
            submitButton: submitButton,
            form: form
        };
    }
    /**
     * Adjuntar event listeners
     */
    attachEventListeners() {
        // Submit del formulario
        this.elements.form.addEventListener('submit', (e) => this.handleSubmit(e));
        // Enter en campos
        this.elements.email.addEventListener('keypress', (e) => this.handleEnterKey(e));
        this.elements.password.addEventListener('keypress', (e) => this.handleEnterKey(e));
        // Validación en tiempo real
        this.elements.email.addEventListener('blur', () => this.validateEmail());
        this.elements.password.addEventListener('blur', () => this.validatePassword());
    }
    /**
     * Manejar submit del formulario
     */
    async handleSubmit(e) {
        e.preventDefault();
        if (this.isSubmitting)
            return;
        // Validar campos
        if (!this.validateForm()) {
            return;
        }
        this.isSubmitting = true;
        this.setLoadingState(true);
        try {
            // Crear FormData con los nombres correctos
            const formData = new URLSearchParams();
            formData.append('EmailOrAlias', this.elements.email.value);
            formData.append('Password', this.elements.password.value);
            // Obtener token antiforgery
            const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
            if (tokenInput && tokenInput.value) {
                formData.append('__RequestVerificationToken', tokenInput.value);
            }
            const response = await fetch('/Login/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: formData.toString(),
                credentials: 'same-origin'
            });
            if (response.ok) {
                // Si la respuesta es redirect (login exitoso)
                if (response.redirected) {
                    // Mostrar mensaje de éxito
                    this.showSuccess('Autenticación exitosa. Cargando sistema...');
                    // Fade out del formulario
                    this.elements.form.style.transition = 'opacity 0.5s ease';
                    this.elements.form.style.opacity = '0.5';
                    // Esperar un momento antes de redirigir
                    setTimeout(() => {
                        // Fade out completo
                        document.body.style.transition = 'opacity 0.3s ease';
                        document.body.style.opacity = '0';
                        setTimeout(() => {
                            window.location.href = response.url;
                        }, 300);
                    }, 800);
                }
                else {
                    // Si es JSON, procesarlo
                    const contentType = response.headers.get('content-type');
                    if (contentType && contentType.includes('application/json')) {
                        const data = await response.json();
                        if (data.success) {
                            this.showSuccess('Autenticación exitosa. Cargando...');
                            setTimeout(() => {
                                window.location.href = data.redirectUrl || '/Home';
                            }, 1000);
                        }
                        else {
                            this.showError(data.message || 'Credenciales inválidas');
                        }
                    }
                    else {
                        // Si es HTML, recargar la página para mostrar errores del servidor
                        window.location.reload();
                    }
                }
            }
            else {
                this.showError('Error al iniciar sesión. Por favor intente nuevamente.');
            }
        }
        catch (error) {
            console.error('Error:', error);
            this.showError('Error de conexión. Por favor verifique su conexión a internet.');
        }
        finally {
            this.isSubmitting = false;
            this.setLoadingState(false);
        }
    }
    /**
     * Manejar tecla Enter
     */
    handleEnterKey(e) {
        if (e.key === 'Enter') {
            e.preventDefault();
            this.elements.form.dispatchEvent(new Event('submit'));
        }
    }
    /**
     * Validar formulario completo
     */
    validateForm() {
        const emailValid = this.validateEmail();
        const passwordValid = this.validatePassword();
        return emailValid && passwordValid;
    }
    /**
     * Validar email
     */
    validateEmail() {
        const email = this.elements.email.value.trim();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!email) {
            this.showFieldError(this.elements.email, 'El correo electrónico es requerido');
            return false;
        }
        if (!emailRegex.test(email)) {
            this.showFieldError(this.elements.email, 'Ingrese un correo electrónico válido');
            return false;
        }
        this.clearFieldError(this.elements.email);
        return true;
    }
    /**
     * Validar contraseña
     */
    validatePassword() {
        const password = this.elements.password.value;
        if (!password) {
            this.showFieldError(this.elements.password, 'La contraseña es requerida');
            return false;
        }
        if (password.length < 6) {
            this.showFieldError(this.elements.password, 'La contraseña debe tener al menos 6 caracteres');
            return false;
        }
        this.clearFieldError(this.elements.password);
        return true;
    }
    /**
     * Mostrar error en campo
     */
    showFieldError(input, message) {
        input.classList.add('is-invalid');
        // Buscar o crear elemento de error
        let errorElement = input.parentElement?.querySelector('.invalid-feedback');
        if (!errorElement) {
            errorElement = document.createElement('div');
            errorElement.className = 'invalid-feedback';
            input.parentElement?.appendChild(errorElement);
        }
        errorElement.textContent = message;
    }
    /**
     * Limpiar error de campo
     */
    clearFieldError(input) {
        input.classList.remove('is-invalid');
        const errorElement = input.parentElement?.querySelector('.invalid-feedback');
        if (errorElement) {
            errorElement.remove();
        }
    }
    /**
     * Mostrar mensaje de error general
     */
    showError(message) {
        // Buscar o crear alert
        let alertElement = document.querySelector('.alert-danger');
        if (!alertElement) {
            alertElement = document.createElement('div');
            alertElement.className = 'alert alert-danger alert-dismissible fade show';
            alertElement.innerHTML = `
                <span class="error-message">${message}</span>
                <button type="button" class="close" data-dismiss="alert">
                    <span>&times;</span>
                </button>
            `;
            this.elements.form.parentElement?.insertBefore(alertElement, this.elements.form);
        }
        else {
            const messageElement = alertElement.querySelector('.error-message');
            if (messageElement) {
                messageElement.textContent = message;
            }
        }
        // Auto-ocultar después de 5 segundos
        setTimeout(() => {
            alertElement.remove();
        }, 5000);
    }
    /**
     * Mostrar mensaje de éxito
     */
    showSuccess(message) {
        // Remover errores previos
        const existingError = document.querySelector('.alert-danger');
        if (existingError)
            existingError.remove();
        // Crear alert de éxito
        const alertElement = document.createElement('div');
        alertElement.className = 'alert alert-success fade-in';
        alertElement.style.cssText = 'animation: fadeIn 0.5s ease; margin-bottom: 20px;';
        alertElement.innerHTML = `
            <i class="fa fa-check-circle"></i> ${message}
            <div class="spinner-border spinner-border-sm float-right" role="status" style="float: right;">
                <span class="sr-only">Cargando...</span>
            </div>
        `;
        this.elements.form.parentElement?.insertBefore(alertElement, this.elements.form);
    }
    /**
     * Establecer estado de carga
     */
    setLoadingState(loading) {
        if (loading) {
            this.elements.submitButton.disabled = true;
            this.elements.submitButton.innerHTML = '<span class="spinner-border spinner-border-sm mr-2"></span>Ingresando...';
            this.elements.email.disabled = true;
            this.elements.password.disabled = true;
        }
        else {
            this.elements.submitButton.disabled = false;
            this.elements.submitButton.innerHTML = 'Ingresar';
            this.elements.email.disabled = false;
            this.elements.password.disabled = false;
        }
    }
}
// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', () => {
    new LoginManager();
});
//# sourceMappingURL=login.js.map