// ============================================================================
// Archivo: employee-form.ts
// Proyecto: RH365.WebMVC
// Ruta: TS/Employees/employee-form.ts
// Descripcion: 
//   - Formulario dinamico para Crear/Editar Empleados
//   - Tab General: Campos de negocio en LAYOUT DE 3 COLUMNAS
//   - Tab Auditoria: Campos ISO 27001 en LAYOUT DE 1 COLUMNA
//   - Enums definidos localmente basados en GlobalsEnum.cs
//   - Renderizado separado para cada tab
//   - Validacion cliente + servidor
//   - Integracion con API REST (/api/Employees)
//   - Labels a la izquierda de los campos
// Estandar: ISO 27001 - Trazabilidad y validacion de datos
// Fecha actualizacion: 2025
// ============================================================================

(function () {
    // ========================================================================
    // CONFIGURACION GLOBAL Y CONTEXTO
    // ========================================================================
    const w: any = window;
    const d: Document = document;
    const $: any = w.jQuery || w.$;

    const apiBase: string = w.RH365.urls.apiBase;
    const pageContainer = d.querySelector("#employee-form-page");

    if (!pageContainer) return;

    const token: string = pageContainer.getAttribute("data-token") || "";
    const dataareaId: string = pageContainer.getAttribute("data-dataarea") || "DAT";
    const userRefRecID: number = parseInt(pageContainer.getAttribute("data-user") || "0", 10);
    let recId: number = parseInt(pageContainer.getAttribute("data-recid") || "0", 10);
    let isNew: boolean = pageContainer.getAttribute("data-isnew") === "true";

    // ========================================================================
    // INTERFACES Y TIPOS
    // ========================================================================

    /**
     * Configuracion de un campo del formulario.
     * Define como se debe renderizar y validar cada campo.
     * ISO 27001: Estructura de campos con validacion y trazabilidad
     */
    interface FieldConfig {
        field: string;
        label: string;
        type: 'text' | 'number' | 'date' | 'datetime' | 'time' | 'textarea' | 'select' | 'checkbox' | 'enum';
        required?: boolean;
        maxLength?: number;
        enumType?: string;  // Nombre del enum en GlobalsEnum.cs
        placeholder?: string;
        helpText?: string;
        readonly?: boolean;
        column?: 'left' | 'center' | 'right';  // Para tab General con 3 columnas
    }

    /**
     * Interface para opciones de enums
     * ISO 27001: Estructura de datos de catalogos
     */
    interface EnumOption {
        value: number;
        text: string;
        displayName?: string;
    }

    /**
     * Cache para almacenar enums cargados
     * ISO 27001: Optimizacion de recursos
     */
    const enumsCache: Record<string, EnumOption[]> = {};

    // ========================================================================
    // VALORES DE ENUMS LOCALES (basados en GlobalsEnum.cs)
    // ========================================================================

    /**
     * Definicion de todos los enums del sistema
     * Basado en GlobalsEnum.cs del frontend
     * ISO 27001: Catalogos definidos localmente para el formulario
     */
    const ENUM_VALUES: Record<string, EnumOption[]> = {
        'Gender': [
            { value: 0, text: 'Masculino', displayName: 'Masculino' },
            { value: 1, text: 'Femenino', displayName: 'Femenino' },
            { value: 2, text: 'No especificado', displayName: 'No especificado' }
        ],
        'MaritalStatus': [
            { value: 0, text: 'Casado/a', displayName: 'Casado/a' },
            { value: 1, text: 'Soltero/a', displayName: 'Soltero/a' },
            { value: 2, text: 'Viudo/a', displayName: 'Viudo/a' },
            { value: 3, text: 'Divorciado/a', displayName: 'Divorciado/a' },
            { value: 4, text: 'Separado/a', displayName: 'Separado/a' }
        ],
        'PayMethod': [
            { value: 0, text: 'Efectivo', displayName: 'Efectivo' },
            { value: 1, text: 'Transferencia', displayName: 'Transferencia' }
        ],
        'WorkStatus': [
            { value: 0, text: 'Candidato', displayName: 'Candidato' },
            { value: 1, text: 'Despedido', displayName: 'Despedido' },
            { value: 2, text: 'Empleado', displayName: 'Empleado' },
            { value: 3, text: 'Deshabilitado', displayName: 'Deshabilitado' }
        ],
        'EmployeeAction': [
            { value: 0, text: 'Ninguno', displayName: 'Ninguno' },
            { value: 1, text: 'Desahucio', displayName: 'Desahucio' },
            { value: 2, text: 'Renuncia', displayName: 'Renuncia' },
            { value: 3, text: 'Termino Contrato Temporal', displayName: 'Termino Contrato Temporal' },
            { value: 4, text: 'Muerte', displayName: 'Muerte' },
            { value: 5, text: 'Transferencia', displayName: 'Transferencia' },
            { value: 6, text: 'Termino Contrato Periodo Probatorio', displayName: 'Termino Contrato Periodo Probatorio' },
            { value: 7, text: 'Promocion', displayName: 'Promocion' },
            { value: 8, text: 'Transferencia Departamento', displayName: 'Transferencia Departamento' },
            { value: 9, text: 'Transferencia Empresas', displayName: 'Transferencia Empresas' },
            { value: 10, text: 'Contrato Temporero', displayName: 'Contrato Temporero' },
            { value: 11, text: 'Termino Contrato', displayName: 'Termino Contrato' },
            { value: 12, text: 'Despido', displayName: 'Despido' },
            { value: 13, text: 'Enfermedad', displayName: 'Enfermedad' }
        ],
        'EmployeeType': [
            { value: 0, text: 'Empleado', displayName: 'Empleado' },
            { value: 1, text: 'Contratista', displayName: 'Contratista' }
        ]
    };

    // ========================================================================
    // DEFINICION DE CAMPOS - TAB GENERAL (Campos de Negocio en 3 COLUMNAS)
    // ========================================================================
    const businessFields: FieldConfig[] = [
        // COLUMNA IZQUIERDA - Datos Personales Basicos
        {
            field: 'EmployeeCode',
            label: 'Codigo Empleado',
            type: 'text',
            required: true,
            maxLength: 50,
            placeholder: 'EMP001',
            column: 'left'
        },
        {
            field: 'Name',
            label: 'Nombre',
            type: 'text',
            required: true,
            maxLength: 200,
            column: 'left'
        },
        {
            field: 'LastName',
            label: 'Apellido',
            type: 'text',
            required: true,
            maxLength: 200,
            column: 'left'
        },
        {
            field: 'PersonalTreatment',
            label: 'Tratamiento Personal',
            type: 'text',
            maxLength: 50,
            placeholder: 'Sr., Sra., Dr.',
            column: 'left'
        },
        {
            field: 'BirthDate',
            label: 'Fecha de Nacimiento',
            type: 'date',
            column: 'left'
        },
        {
            field: 'Age',
            label: 'Edad (Calculada)',
            type: 'number',
            readonly: true,
            column: 'left'
        },
        {
            field: 'Gender',
            label: 'Genero',
            type: 'enum',
            required: true,
            enumType: 'Gender',
            column: 'left'
        },
        {
            field: 'MaritalStatus',
            label: 'Estado Civil',
            type: 'enum',
            required: true,
            enumType: 'MaritalStatus',
            column: 'left'
        },
        {
            field: 'DependentsNumbers',
            label: 'Numero de Dependientes',
            type: 'number',
            placeholder: '0',
            column: 'left'
        },
        {
            field: 'Nationality',
            label: 'Nacionalidad',
            type: 'text',
            maxLength: 100,
            column: 'left'
        },

        // COLUMNA CENTRAL - Datos de Seguridad Social y Laborales
        {
            field: 'Nss',
            label: 'NSS',
            type: 'text',
            maxLength: 50,
            column: 'center'
        },
        {
            field: 'Ars',
            label: 'ARS',
            type: 'text',
            maxLength: 100,
            column: 'center'
        },
        {
            field: 'Afp',
            label: 'AFP',
            type: 'text',
            maxLength: 100,
            column: 'center'
        },
        {
            field: 'AdmissionDate',
            label: 'Fecha de Admision',
            type: 'datetime',
            required: true,
            column: 'center'
        },
        {
            field: 'StartWorkDate',
            label: 'Fecha Inicio Laboral',
            type: 'datetime',
            required: true,
            column: 'center'
        },
        {
            field: 'PayMethod',
            label: 'Metodo de Pago',
            type: 'enum',
            required: true,
            enumType: 'PayMethod',
            column: 'center'
        },
        {
            field: 'WorkStatus',
            label: 'Estado Laboral',
            type: 'enum',
            required: true,
            enumType: 'WorkStatus',
            column: 'center'
        },
        {
            field: 'EmployeeAction',
            label: 'Accion sobre Empleado',
            type: 'enum',
            required: true,
            enumType: 'EmployeeAction',
            column: 'center'
        },
        {
            field: 'EmployeeType',
            label: 'Tipo de Empleado',
            type: 'enum',
            required: true,
            enumType: 'EmployeeType',
            column: 'center'
        },

        // COLUMNA DERECHA - Configuraciones y Horarios
        {
            field: 'EmployeeStatus',
            label: 'Estado del Empleado',
            type: 'select',
            required: true,
            column: 'right'
        },
        {
            field: 'WorkFrom',
            label: 'Hora de Entrada',
            type: 'time',
            placeholder: '08:00',
            column: 'right'
        },
        {
            field: 'WorkTo',
            label: 'Hora de Salida',
            type: 'time',
            placeholder: '17:00',
            column: 'right'
        },
        {
            field: 'BreakWorkFrom',
            label: 'Inicio de Descanso',
            type: 'time',
            placeholder: '12:00',
            column: 'right'
        },
        {
            field: 'BreakWorkTo',
            label: 'Fin de Descanso',
            type: 'time',
            placeholder: '13:00',
            column: 'right'
        },
        {
            field: 'HomeOffice',
            label: 'Trabajo Remoto',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'OwnCar',
            label: 'Auto Propio',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'HasDisability',
            label: 'Tiene Discapacidad',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'ApplyForOvertime',
            label: 'Aplica para Horas Extras',
            type: 'checkbox',
            column: 'right'
        },
        {
            field: 'IsFixedWorkCalendar',
            label: 'Horario de Trabajo Fijo',
            type: 'checkbox',
            column: 'right'
        }
    ];

    // ========================================================================
    // DEFINICION DE CAMPOS - TAB AUDITORIA (ISO 27001 en 1 COLUMNA)
    // ========================================================================
    const auditFields: FieldConfig[] = [
        {
            field: 'RecID',
            label: 'RecID (Clave Primaria)',
            type: 'number',
            readonly: true
        },
        {
            field: 'DataareaID',
            label: 'Empresa (DataareaID)',
            type: 'text',
            readonly: true
        },
        {
            field: 'CreatedBy',
            label: 'Creado Por',
            type: 'text',
            readonly: true
        },
        {
            field: 'CreatedOn',
            label: 'Fecha de Creacion',
            type: 'datetime',
            readonly: true
        },
        {
            field: 'ModifiedBy',
            label: 'Modificado Por',
            type: 'text',
            readonly: true
        },
        {
            field: 'ModifiedOn',
            label: 'Fecha de Ultima Modificacion',
            type: 'datetime',
            readonly: true
        }
    ];

    // Variable global para almacenar los datos del empleado
    let employeeData: any = null;

    // ========================================================================
    // UTILIDADES - COMUNICACION CON API
    // ========================================================================

    /**
     * Realiza una peticion HTTP al API con manejo de autenticacion
     * @param url URL completa del endpoint
     * @param options Opciones adicionales para fetch (method, body, etc.)
     * @returns Promise con la respuesta JSON parseada
     * ISO 27001: Comunicacion segura con el servidor mediante tokens
     */
    const fetchJson = async (url: string, options?: RequestInit): Promise<any> => {
        const headers: Record<string, string> = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };

        // Agregar token de autenticacion si existe
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const response = await fetch(url, { ...options, headers });

        // Si la respuesta no es exitosa, lanzar error con el detalle
        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(JSON.stringify(errorData));
        }

        return response.json();
    };

    // ========================================================================
    // GESTION DE ENUMS LOCALES
    // ========================================================================

    /**
     * Carga todos los enums necesarios en el cache
     * ISO 27001: Inicializacion de catalogos locales
     */
    const loadAllEnums = (): void => {
        console.log('📋 Cargando enums locales...');

        // Obtener lista de enums necesarios
        const enumTypes = new Set<string>();
        businessFields.forEach(field => {
            if (field.type === 'enum' && field.enumType) {
                enumTypes.add(field.enumType);
            }
        });

        // Cargar cada enum en el cache
        enumTypes.forEach(enumType => {
            if (ENUM_VALUES[enumType]) {
                enumsCache[enumType] = ENUM_VALUES[enumType];
                console.log(`✅ Enum ${enumType} cargado: ${ENUM_VALUES[enumType].length} opciones`);
            } else {
                console.warn(`⚠️ Enum ${enumType} no definido`);
            }
        });

        console.log('✅ Todos los enums cargados');
    };

    /**
     * Genera opciones para el campo EmployeeStatus (Activo/Inactivo)
     * Este es un campo booleano especial que no viene de un enum
     * ISO 27001: Manejo especial de campos booleanos
     */
    const getEmployeeStatusOptions = (): EnumOption[] => {
        return [
            { value: 1, text: 'Activo', displayName: 'Activo' },
            { value: 0, text: 'Inactivo', displayName: 'Inactivo' }
        ];
    };

    // ========================================================================
    // FUNCIONES DE CALCULO DE EDAD
    // ========================================================================

    /**
     * Calcula la edad basandose en la fecha de nacimiento
     * @param birthDate Fecha de nacimiento
     * @returns Edad en años o null si no hay fecha
     * ISO 27001: Calculo automatico de campos derivados
     */
    const calculateAge = (birthDate: string | Date | null): number | null => {
        if (!birthDate) return null;

        const birth = typeof birthDate === 'string' ? new Date(birthDate) : birthDate;
        if (isNaN(birth.getTime())) return null;

        const today = new Date();
        let age = today.getFullYear() - birth.getFullYear();
        const monthDiff = today.getMonth() - birth.getMonth();

        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birth.getDate())) {
            age--;
        }

        return age >= 0 ? age : null;
    };

    /**
     * Actualiza el campo de edad cuando cambia la fecha de nacimiento
     * ISO 27001: Actualizacion automatica de campos calculados
     */
    const updateAgeField = (): void => {
        const birthDateInput = document.getElementById('BirthDate') as HTMLInputElement;
        const ageInput = document.getElementById('Age') as HTMLInputElement;

        if (birthDateInput && ageInput) {
            const birthDateValue = birthDateInput.value;
            const age = calculateAge(birthDateValue);
            ageInput.value = age !== null ? age.toString() : '';
            console.log('Edad calculada:', age);
        }
    };

    // ========================================================================
    // RENDERIZADO DE CAMPOS
    // ========================================================================

    /**
     * Genera el HTML de un campo segun su configuracion
     * @param config Configuracion del campo
     * @param value Valor actual del campo
     * @param layoutType Tipo de layout: 'general3col' | 'audit1col'
     * @returns String con el HTML del campo
     * ISO 27001: Renderizado consistente con validacion de datos
     */
    const renderField = (config: FieldConfig, value: any, layoutType: 'general3col' | 'audit1col'): string => {
        const fieldId = config.field;
        const fieldName = config.field;

        // Ajustar clases segun el tipo de layout
        let labelClass = '';
        let inputContainerClass = '';

        if (layoutType === 'general3col') {
            // Para 3 columnas, labels mas estrechos
            labelClass = 'control-label col-md-5 col-sm-5 col-xs-12';
            inputContainerClass = 'col-md-7 col-sm-7 col-xs-12';
        } else if (layoutType === 'audit1col') {
            // Para 1 columna en auditoria
            labelClass = 'control-label col-md-3 col-sm-3 col-xs-12';
            inputContainerClass = 'col-md-6 col-sm-6 col-xs-12';
        }

        const requiredMark = config.required ? '<span class="required">*</span>' : '';
        const readonlyAttr = config.readonly ? 'readonly' : '';
        const requiredAttr = config.required ? 'required' : '';

        let inputHtml = '';
        let displayValue = value ?? '';

        // Generar input segun el tipo de campo
        switch (config.type) {
            case 'textarea':
                inputHtml = `<textarea id="${fieldId}" name="${fieldName}" class="form-control" rows="3" maxlength="${config.maxLength || 500}" ${readonlyAttr} ${requiredAttr}>${displayValue}</textarea>`;
                break;

            case 'enum':
                // Cargar opciones del enum desde el cache
                const enumOptions = config.enumType ? (enumsCache[config.enumType] || []) : [];

                // Agregar opcion vacia para campos no requeridos
                const emptyOption = config.required ? '' : '<option value="">-- Seleccione --</option>';

                const enumOptionsHtml = emptyOption + enumOptions.map(opt => {
                    const selected = displayValue !== undefined && displayValue !== null &&
                        displayValue.toString() === opt.value.toString() ? 'selected' : '';
                    return `<option value="${opt.value}" ${selected}>${opt.displayName || opt.text}</option>`;
                }).join('');

                inputHtml = `<select id="${fieldId}" name="${fieldName}" class="form-control" ${readonlyAttr ? 'disabled' : ''} ${requiredAttr}>${enumOptionsHtml}</select>`;
                break;

            case 'select':
                // Para campos select que no son enums (como EmployeeStatus)
                let options: EnumOption[] = [];

                if (config.field === 'EmployeeStatus') {
                    options = getEmployeeStatusOptions();
                }

                const selectEmptyOption = config.required ? '' : '<option value="">-- Seleccione --</option>';

                const selectOptionsHtml = selectEmptyOption + options.map(opt => {
                    const selected = displayValue === true && opt.value === 1 ||
                        displayValue === false && opt.value === 0 ||
                        displayValue === opt.value ? 'selected' : '';
                    return `<option value="${opt.value}" ${selected}>${opt.text}</option>`;
                }).join('');

                inputHtml = `<select id="${fieldId}" name="${fieldName}" class="form-control" ${readonlyAttr ? 'disabled' : ''} ${requiredAttr}>${selectOptionsHtml}</select>`;
                break;

            case 'checkbox':
                const checked = displayValue === true || displayValue === 'true' || displayValue === 1 ? 'checked' : '';
                inputHtml = `<input type="checkbox" id="${fieldId}" name="${fieldName}" class="flat" ${checked} ${readonlyAttr ? 'disabled' : ''}>`;
                break;

            case 'datetime':
                // Formatear datetime para visualizacion en modo readonly
                if (config.readonly && displayValue && typeof displayValue === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(displayValue)) {
                    const dt = new Date(displayValue);
                    displayValue = dt.toLocaleString('es-DO', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        hour: '2-digit',
                        minute: '2-digit'
                    });
                    inputHtml = `<input type="text" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr}>`;
                } else {
                    // Para campos editables, usar datetime-local
                    if (displayValue && typeof displayValue === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(displayValue)) {
                        displayValue = displayValue.substring(0, 16);
                    }
                    inputHtml = `<input type="datetime-local" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                }
                break;

            case 'date':
                // Extraer solo la fecha (YYYY-MM-DD) si viene datetime
                if (displayValue && typeof displayValue === 'string' && /^\d{4}-\d{2}-\d{2}T/.test(displayValue)) {
                    displayValue = displayValue.split('T')[0];
                }
                inputHtml = `<input type="date" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                break;

            case 'time':
                // Formatear tiempo para input time
                if (displayValue && typeof displayValue === 'string') {
                    if (displayValue.includes('T')) {
                        displayValue = displayValue.split('T')[1].substring(0, 5);
                    } else {
                        displayValue = displayValue.substring(0, 5);
                    }
                }
                inputHtml = `<input type="time" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr} placeholder="${config.placeholder || ''}">`;
                break;

            case 'number':
                inputHtml = `<input type="number" id="${fieldId}" name="${fieldName}" class="form-control" value="${displayValue}" ${readonlyAttr} ${requiredAttr}>`;
                break;

            default: // text
                inputHtml = `<input type="text" id="${fieldId}" name="${fieldName}" class="form-control" maxlength="${config.maxLength || 255}" value="${displayValue}" placeholder="${config.placeholder || ''}" ${readonlyAttr} ${requiredAttr}>`;
                break;
        }

        // Agregar texto de ayuda si existe
        const helpTextHtml = config.helpText ? `<span class="help-block">${config.helpText}</span>` : '';

        // Retornar el HTML completo del form-group
        return `
            <div class="form-group">
                <label class="${labelClass}" for="${fieldId}">
                    ${config.label} ${requiredMark}
                </label>
                <div class="${inputContainerClass}">
                    ${inputHtml}
                    ${helpTextHtml}
                </div>
            </div>
        `;
    };

    // ========================================================================
    // CARGA DE DATOS DEL EMPLEADO
    // ========================================================================

    /**
     * Carga los datos del empleado desde el API (solo si es edicion)
     * En modo creacion, renderiza los formularios vacios
     * ISO 27001: Carga segura de datos con validacion
     */
    const loadEmployeeData = async (): Promise<void> => {
        if (isNew) {
            // Modo creacion: renderizar formularios vacios
            renderBusinessForm({});
            renderAuditForm({});
            setupFieldEventListeners();
            return;
        }

        // Modo edicion: cargar datos desde el API
        try {
            const url = `${apiBase}/Employees/${recId}`;
            employeeData = await fetchJson(url);

            // Calcular edad si hay fecha de nacimiento
            if (employeeData.BirthDate) {
                employeeData.Age = calculateAge(employeeData.BirthDate);
            }

            // Renderizar ambos formularios con los datos cargados
            renderBusinessForm(employeeData);
            renderAuditForm(employeeData);
            setupFieldEventListeners();
        } catch (error) {
            (w as any).ALERTS.error('Error al cargar los datos del empleado', 'Error');

            // Renderizar formularios vacios en caso de error
            renderBusinessForm({});
            renderAuditForm({});
            setupFieldEventListeners();
        }
    };

    // ========================================================================
    // RENDERIZADO DE FORMULARIOS
    // ========================================================================

    /**
     * Renderiza el formulario de campos de negocio en LAYOUT DE 3 COLUMNAS
     * @param data Datos del empleado a mostrar
     * ISO 27001: Presentacion organizada de campos con validacion
     */
    const renderBusinessForm = (data: any): void => {
        const containerLeft = $('#dynamic-fields-col-left');
        const containerCenter = $('#dynamic-fields-col-center');
        const containerRight = $('#dynamic-fields-col-right');

        containerLeft.empty();
        containerCenter.empty();
        containerRight.empty();

        // Renderizar campos en columna izquierda
        businessFields
            .filter(config => config.column === 'left')
            .forEach(config => {
                let value = data[config.field];

                // Para el campo Age, calcular desde BirthDate
                if (config.field === 'Age' && data.BirthDate) {
                    value = calculateAge(data.BirthDate);
                }

                const fieldHtml = renderField(config, value, 'general3col');
                containerLeft.append(fieldHtml);
            });

        // Renderizar campos en columna central
        businessFields
            .filter(config => config.column === 'center')
            .forEach(config => {
                const value = data[config.field];
                const fieldHtml = renderField(config, value, 'general3col');
                containerCenter.append(fieldHtml);
            });

        // Renderizar campos en columna derecha
        businessFields
            .filter(config => config.column === 'right')
            .forEach(config => {
                const value = data[config.field];
                const fieldHtml = renderField(config, value, 'general3col');
                containerRight.append(fieldHtml);
            });

        // Inicializar iCheck para checkboxes si existe el plugin
        if ($.fn.iCheck) {
            $('.flat').iCheck({
                checkboxClass: 'icheckbox_flat-green'
            });
        }
    };

    /**
     * Renderiza el formulario de campos de auditoria en LAYOUT DE 1 COLUMNA
     * @param data Datos del empleado a mostrar
     * ISO 27001: Campos de auditoria de solo lectura para trazabilidad
     */
    const renderAuditForm = (data: any): void => {
        const container = $('#audit-fields-container');
        container.empty();

        // Si es modo creacion, mostrar mensaje informativo
        if (isNew) {
            container.html(`
                <div class="alert alert-warning" role="alert">
                    <i class="fa fa-info-circle"></i>
                    <strong>Modo Creacion:</strong> 
                    Los campos de auditoria se generaran automaticamente despues de guardar el empleado.
                </div>
            `);
            return;
        }

        // Renderizar TODOS los campos de auditoria en una sola columna
        auditFields.forEach(config => {
            const value = data[config.field];
            const fieldHtml = renderField(config, value, 'audit1col');
            container.append(fieldHtml);
        });
    };

    // ========================================================================
    // CONFIGURACION DE EVENT LISTENERS
    // ========================================================================

    /**
     * Configura los event listeners para campos que requieren actualizacion dinamica
     * ISO 27001: Control de eventos para campos calculados
     */
    const setupFieldEventListeners = (): void => {
        // Event listener para calcular edad cuando cambia fecha de nacimiento
        $(document).on('change', '#BirthDate', function () {
            updateAgeField();
        });
    };

    // ========================================================================
    // CAPTURA DE DATOS DEL FORMULARIO
    // ========================================================================

    /**
     * Obtiene los datos del formulario de negocio para enviar al API
     * SOLO captura campos editables del Tab General
     * @returns Objeto con los datos del formulario
     * ISO 27001: Captura y validacion de datos antes del envio
     */
    const getFormData = (): any => {
        const formData: any = {};

        // Iterar SOLO sobre businessFields (no auditFields)
        businessFields.forEach(config => {
            const field = config.field;
            const $input = $(`#${field}`);

            // Saltar campos readonly (como Age) - NO SE ENVIAN AL API
            if (config.readonly || field === 'Age') {
                return;
            }

            if ($input.length) {
                if (config.type === 'checkbox') {
                    // Capturar valor booleano de checkbox
                    formData[field] = $input.is(':checked');
                } else if (config.type === 'select' && field === 'EmployeeStatus') {
                    // Convertir a boolean para EmployeeStatus
                    const val = $input.val();
                    formData[field] = val === '1' || val === 1;
                } else if (config.type === 'enum') {
                    // Convertir valores de enums a enteros
                    const val = $input.val();
                    formData[field] = val !== '' && val !== null ? parseInt(val) : null;
                } else if (config.type === 'number' || field === 'DependentsNumbers') {
                    // Convertir a numero o null
                    const val = $input.val();
                    formData[field] = val ? parseInt(val) : null;
                } else if (config.type === 'datetime') {
                    // Convertir datetime a ISO string
                    const val = $input.val();
                    formData[field] = val ? new Date(val).toISOString() : null;
                } else if (config.type === 'date') {
                    // Convertir date a ISO string
                    const val = $input.val();
                    formData[field] = val ? new Date(val + 'T00:00:00').toISOString() : null;
                } else if (config.type === 'time') {
                    // Mantener tiempo como string
                    const val = $input.val();
                    formData[field] = val || null;
                } else {
                    // Capturar como string o null
                    const val = $input.val();
                    formData[field] = val || null;
                }
            }
        });

        return formData;
    };

    // ========================================================================
    // GUARDADO DE EMPLEADO
    // ========================================================================

    /**
     * Guarda el empleado en el API (POST para crear, PUT para actualizar)
     * PERMANECE EN EL FORMULARIO despues de guardar
     * ISO 27001: Persistencia segura de datos con validacion
     */
    const saveEmployee = async (): Promise<void> => {
        const formData = getFormData();

        try {
            // Determinar URL y metodo segun el modo (crear/editar)
            const url = isNew ? `${apiBase}/Employees` : `${apiBase}/Employees/${recId}`;
            const method = isNew ? 'POST' : 'PUT';

            // Construir payload con los campos necesarios (SIN Age)
            const payload = {
                EmployeeCode: formData.EmployeeCode,
                Name: formData.Name,
                LastName: formData.LastName,
                PersonalTreatment: formData.PersonalTreatment || null,
                BirthDate: formData.BirthDate || null,
                Gender: formData.Gender,
                DependentsNumbers: formData.DependentsNumbers || null,
                MaritalStatus: formData.MaritalStatus,
                Nss: formData.Nss || null,
                Ars: formData.Ars || null,
                Afp: formData.Afp || null,
                AdmissionDate: formData.AdmissionDate,
                StartWorkDate: formData.StartWorkDate,
                PayMethod: formData.PayMethod,
                WorkStatus: formData.WorkStatus,
                EmployeeAction: formData.EmployeeAction,
                EmployeeType: formData.EmployeeType,
                EmployeeStatus: formData.EmployeeStatus,
                CountryRecId: 0,
                DisabilityTypeRecId: null,
                EducationLevelRecId: null,
                OccupationRecId: null,
                HomeOffice: formData.HomeOffice || false,
                OwnCar: formData.OwnCar || false,
                HasDisability: formData.HasDisability || false,
                ApplyForOvertime: formData.ApplyForOvertime || false,
                IsFixedWorkCalendar: formData.IsFixedWorkCalendar || false,
                WorkFrom: formData.WorkFrom || null,
                WorkTo: formData.WorkTo || null,
                BreakWorkFrom: formData.BreakWorkFrom || null,
                BreakWorkTo: formData.BreakWorkTo || null,
                Nationality: formData.Nationality || null
            };

            // Debug en consola
            console.log('FormData capturado:', formData);
            console.log('Enviando payload:', payload);

            // Enviar peticion al API
            const response = await fetchJson(url, {
                method: method,
                body: JSON.stringify(payload)
            });

            // Mostrar alerta de exito
            (w as any).ALERTS.ok(
                isNew ? 'Empleado creado exitosamente' : 'Empleado actualizado exitosamente',
                'Exito'
            );

            // Si era nuevo, actualizar el contexto para convertirlo en edicion
            if (isNew) {
                isNew = false;
                recId = response.RecID || response.recID || response.Id || response.id;

                // Actualizar los atributos del contenedor
                pageContainer.setAttribute('data-isnew', 'false');
                pageContainer.setAttribute('data-recid', recId.toString());

                // Actualizar el titulo
                document.querySelector('h3').textContent = 'Editar Empleado';

                // Recargar el empleado con los datos completos (incluidos campos de auditoria)
                await loadEmployeeData();
            } else {
                // Si es edicion, solo actualizar los datos en memoria
                employeeData = response;

                // Recalcular la edad si hay fecha de nacimiento
                if (employeeData.BirthDate) {
                    updateAgeField();
                }
            }

        } catch (error: any) {
            console.error('Error al guardar:', error);
            let errorMessage = 'Error al guardar el empleado';

            // Intentar parsear el mensaje de error del API
            try {
                const errorData = JSON.parse(error.message);

                // Si hay errores de validacion, construir mensaje detallado
                if (errorData.errors) {
                    const errorsArray: string[] = [];
                    for (const key in errorData.errors) {
                        if (errorData.errors.hasOwnProperty(key)) {
                            const errList = errorData.errors[key];
                            if (Array.isArray(errList)) {
                                for (let i = 0; i < errList.length; i++) {
                                    errorsArray.push(errList[i]);
                                }
                            } else {
                                errorsArray.push(errList);
                            }
                        }
                    }
                    errorMessage = errorsArray.join(', ');
                } else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            } catch {
                // Si no se puede parsear, usar el mensaje original
                errorMessage = error.message || errorMessage;
            }

            // Mostrar alerta de error
            (w as any).ALERTS.error(errorMessage, 'Error');
        }
    };

    // ========================================================================
    // EVENT HANDLERS
    // ========================================================================

    /**
     * Manejador del boton Guardar
     * Valida el formulario y ejecuta el guardado
     * ISO 27001: Validacion antes de la persistencia
     */
    $('#btn-save').on('click', async () => {
        const form = document.getElementById('frm-employee') as HTMLFormElement;

        // Validar campos requeridos del formulario
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        // Ejecutar guardado
        await saveEmployee();
    });

    // ========================================================================
    // INICIALIZACION
    // ========================================================================

    /**
     * Funcion de inicializacion que se ejecuta cuando el DOM esta listo
     * Carga los enums locales y renderiza los formularios
     * ISO 27001: Inicializacion controlada y segura
     */
    $(async function () {
        try {
            console.log('🚀 Iniciando formulario de empleados...');

            // Cargar enums locales (sin llamadas al API)
            loadAllEnums();

            // Cargar datos del empleado (si es edicion)
            await loadEmployeeData();

            console.log('✅ Formulario inicializado correctamente');
        } catch (error) {
            console.error('Error al inicializar el formulario:', error);
            (w as any).ALERTS.error('Error al inicializar el formulario', 'Error');
        }
    });
})();