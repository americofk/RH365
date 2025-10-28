# Módulo de Préstamos (Loans) - RH365

Este paquete contiene todos los archivos necesarios para implementar el módulo completo de Préstamos en RH365.

## Estructura del Paquete

```
RH365_Loans_Module/
├── Models/                      # Modelos de dominio (.NET)
│   ├── LoanResponse.cs
│   ├── LoanListResponse.cs
│   ├── CreateLoanRequest.cs
│   └── UpdateLoanRequest.cs
├── Services/                    # Servicios de infraestructura
│   └── LoanService.cs
├── Controllers/                 # Controladores MVC
│   └── LoanController.cs
├── Views/Loan/                  # Vistas Razor
│   ├── LP_Loans.cshtml         # Lista de préstamos
│   └── NewEdit_Loan.cshtml     # Formulario crear/editar
└── TypeScript/Loans/            # Scripts TypeScript
    ├── loans-dt.ts             # DataTable de préstamos
    └── loan-form.ts            # Formulario dinámico
```

## Instalación

### 1. Modelos (.NET Core)
Copiar los archivos de `Models/` a:
```
RH365.Core/Domain/Models/Loan/
```

### 2. Servicio
Copiar `LoanService.cs` a:
```
RH365.Infrastructure/Services/
```

Registrar el servicio en `Program.cs`:
```csharp
builder.Services.AddHttpClient<ILoanService, LoanService>();
```

Agregar URL en `UrlsServices.cs`:
```csharp
["Loans"] = $"{BaseUrl}/Loans",
["Loans.Enabled"] = $"{BaseUrl}/Loans/enabled",
```

### 3. Controlador
Copiar `LoanController.cs` a:
```
RH365.WebMVC/Controllers/
```

### 4. Vistas
Copiar archivos de `Views/Loan/` a:
```
RH365.WebMVC/Views/Loan/
```

### 5. TypeScript
Copiar archivos de `TypeScript/Loans/` a:
```
RH365.WebMVC/TS/Loans/
```

Compilar TypeScript:
```bash
tsc TS/Loans/loans-dt.ts --outDir wwwroot/js/Loans
tsc TS/Loans/loan-form.ts --outDir wwwroot/js/Loans
```

## Características Implementadas

### Lista de Préstamos (LP_Loans)
- ✅ DataTable con paginación
- ✅ Búsqueda y filtros
- ✅ Gestión de vistas personalizadas por usuario
- ✅ Exportación a CSV
- ✅ Selección múltiple con checkboxes
- ✅ Doble clic para editar
- ✅ Botones: Nuevo, Editar, Eliminar
- ✅ Alertas con PNotify
- ✅ Modal de confirmación para eliminar

### Formulario (NewEdit_Loan)
- ✅ Modo crear/editar
- ✅ Layout de 2 columnas
- ✅ Tab General (campos de negocio)
- ✅ Tab Auditoría (ISO 27001)
- ✅ Campos dinámicos desde TypeScript
- ✅ Validación cliente y servidor
- ✅ Integración completa con API REST

### Campos del Préstamo
**Tab General:**
- LoanCode (requerido)
- Name (requerido)
- Description
- LedgerAccount
- ValidFrom (fecha)
- ValidTo (fecha)
- MultiplyAmount
- PayFrecuency
- IndexBase
- LoanStatus (Activo/Inactivo)

**Tab Auditoría (solo lectura):**
- RecID
- ID
- DataareaID
- CreatedBy/CreatedOn
- ModifiedBy/ModifiedOn

## Buenas Prácticas Aplicadas

1. **Arquitectura limpia**: Separación de responsabilidades
2. **Sin console.log**: Solo alertas visuales con PNotify
3. **Modales de confirmación**: Para acciones destructivas
4. **TypeScript tipado**: Interfaces y tipos definidos
5. **Responsive design**: Compatible con móviles y tablets
6. **ISO 27001**: Trazabilidad completa de auditoría
7. **Doble clic**: Edición rápida desde la tabla

## Dependencias

- Bootstrap 3
- Font Awesome
- jQuery + jQuery UI
- DataTables
- iCheck
- PNotify
- TypeScript compiler

---
**Versión:** 1.0.0  
**Fecha:** Octubre 2025
