using System;
using System.Collections.Generic;
using System.Text;

namespace RH365.Infrastructure.Services
{
    public class UrlsServices
    {
        #region Url
        public string urlBaseOne = "http://localhost:9595/api"; // local

        #endregion

        public string GetUrl(string _type)
        {
            string UrlResponse = string.Empty;

            switch (_type)
            {
                //Historial de empleados
                case "CalendarHoliday":
                    UrlResponse = $"{urlBaseOne}calendarholidays";
                    break;
                //GeneralConfig
                case "GeneralConfig":
                    UrlResponse = $"{urlBaseOne}generalconfigs";
                    break;

                //Historial de empleados
                case "HorarioEmpleado":
                    UrlResponse = $"{urlBaseOne}employeeworkcalendars";
                    break;

                //Historial de empleados
                case "Employeehistories":
                    UrlResponse = $"{urlBaseOne}employeehistories";
                    break;

                //Configuración reportes
                case "ReportConfig":
                    UrlResponse = $"{urlBaseOne}reportconfig";
                    break;

                //Contratar empleado
                case "HireEmployee":
                    UrlResponse = $"{urlBaseOne}employees";
                    break;

                //BatchHistory
                case "BatchHistory":
                    UrlResponse = $"{urlBaseOne}importbatch";
                    break;

                //Lista de candidatos a empleados
                case "Candidate":
                    UrlResponse = $"{urlBaseOne}employees/candidate";
                    break;

                //Lista empleados dados de baja
                case "Dissmis":
                    UrlResponse = $"{urlBaseOne}employees/dissmis";
                    break;
                //Lista de monedas
                case "Currency":
                    UrlResponse = $"{urlBaseOne}currencys";
                    break;
                //validar usuario del sistema
                case "ValidateUser":
                    //UrlResponse = $"{urlBase}api/login";
                    UrlResponse = $"{urlBaseOne}login";
                    break;
                //menu del sistema
                case "Menu":
                    UrlResponse = $"{urlBaseOne}menusapp";
                    break;
                //roles
                case "Roles":
                    UrlResponse = $"{urlBaseOne}roles";
                    break;
                //Lista de departamentos inactivos
                case "Departmentdisabled":
                    UrlResponse = $"{urlBaseOne}departaments/disabled";
                    break;
                //Lista de departamentos activos 
                case "Departments":
                    UrlResponse = $"{urlBaseOne}departaments/enabled";
                    break;
                //Lista de usuarios del sistema
                case "User":
                    //UrlResponse = $"{urlBase}api/users";
                    UrlResponse = $"{urlBaseOne}users";
                    break;
                //Formato de codigo para numero y fechas
                case "FormatCode":
                    UrlResponse = $"{urlBaseOne}regionalcodes";
                    break;
                //Todas las empresas
                case "Company":
                    UrlResponse = $"{urlBaseOne}companies";
                    break;

                //Opciones del menu de un usuario
                case "MenuAssigned":
                    UrlResponse = $"{urlBaseOne}menustouser";
                    break;
                //Empresas de un usuario
                case "Companiestouser":
                    UrlResponse = $"{urlBaseOne}companiestouser";
                    break;

                //cargar imagen de usuario
                case "Uploadimageuser":
                    UrlResponse = $"{urlBaseOne}users/uploadimageuser";
                    break;
                //descargar imagen de usuario
                case "Downloadimageuser":
                    UrlResponse = $"{urlBaseOne}users/downloadimageuser";
                    break;
                //Solicitud de contraseña temporal
                case "Requestchangepassword":
                    UrlResponse = $"{urlBaseOne}requestchangepassword";
                    break;

                //Envio de contraseña
                case "Sendnewpassword":
                    //UrlResponse = $"{urlBase}api/sendnewpassword";
                    UrlResponse = $"{urlBaseOne}sendnewpassword";
                    break;
                //Tipo de cursos
                case "Coursetypes":
                    UrlResponse = $"{urlBaseOne}coursetypes";
                    break;
                //Instructor de cursos
                case "Instructors":
                    UrlResponse = $"{urlBaseOne}instructors";
                    break;
                //ubicación de cursos
                case "CourseLocation":
                    UrlResponse = $"{urlBaseOne}courselocations";
                    break;
                //salon de cursos
                case "ClassRoom":
                    UrlResponse = $"{urlBaseOne}classrooms";
                    break;

                //para cambiar las opciones por defecto de un usuario
                case "UserOptions":
                    //UrlResponse = $"{urlBase}api/users/options";
                    UrlResponse = $"{urlBaseOne}users/options";
                    break;

                //Cursos
                case "Course":
                    UrlResponse = $"{urlBaseOne}courses";
                    break;

                //Puestos activos
                case "PositionsEnabled":
                    UrlResponse = $"{urlBaseOne}positions/enabled";
                    break;

                //Puestos inactivos
                case "PositionsDisabled":
                    UrlResponse = $"{urlBaseOne}positions/disabled";
                    break;


                //Cargos activos
                case "JobsEnabled":
                    UrlResponse = $"{urlBaseOne}jobs/enabled";
                    break;

                //Cargos inactivos
                case "JobsDisabled":
                    UrlResponse = $"{urlBaseOne}jobs/disabled";
                    break;

                //Lista de vacantes
                case "Vacants":
                    UrlResponse = $"{urlBaseOne}positions/vacants";
                    break;

                //Lista de requisitos de departamentos
                case "Positionrequirements":
                    UrlResponse = $"{urlBaseOne}positionrequirements";
                    break;

                //Lista de empleados activos
                case "EmployeesEnabled":
                    UrlResponse = $"{urlBaseOne}employees/enabled";
                    break;

                //Lista de empleados inactivos
                case "EmployeesDisabled":
                    UrlResponse = $"{urlBaseOne}employees/disabled";
                    break;

                //Lista de empleados inactivos
                case "Employees":
                    UrlResponse = $"{urlBaseOne}employees";
                    break;

                //Direcciones de empleados
                case "Employeeaddress":
                    UrlResponse = $"{urlBaseOne}employeeaddress";
                    break;

                //Información de contacto de empleados
                case "EmployeeContacts":
                    UrlResponse = $"{urlBaseOne}employeecontactinfs";
                    break;

                //Información de cuentas bancarias de empleados
                case "EmployeeBankAccount":
                    UrlResponse = $"{urlBaseOne}employeebankaccounts";
                    break;

                //Lista de paises
                case "Countries":
                    UrlResponse = $"{urlBaseOne}countries";
                    break;

                //Lista codigos de ganancias
                case "Earningcodes":
                    UrlResponse = $"{urlBaseOne}earningcodes/enabled";
                    break;

                //codigos de ganancias para versiones
                case "EarningcodesVersion":
                    UrlResponse = $"{urlBaseOne}earningcodes/version";
                    break;

                //Lista codigos de ganancias deshabilitados
                case "Earningcodedisabled":
                    UrlResponse = $"{urlBaseOne}earningcodes/disabled";
                    break;

                //Lista codigos de deducciones activos
                case "Deductioncodes":
                    UrlResponse = $"{urlBaseOne}deductioncodes/enabled";
                    break;

                //Lista codigos de deducciones inactivos
                case "DeductioncodeDisabled":
                    UrlResponse = $"{urlBaseOne}deductioncodes/disabled";
                    break;

                //Codigos de ganancias por empleados
                case "Employeeearningcodes":
                    UrlResponse = $"{urlBaseOne}employeeearningcodes";
                    break;

                //Codigos de deduciones por empleados
                case "Employeedeductioncodes":
                    UrlResponse = $"{urlBaseOne}employeedeductioncodes";
                    break;

                //Departamentos por empleados
                case "Employeedepartments":
                    UrlResponse = $"{urlBaseOne}employeedepartments";
                    break;

                //Puesto por empleados
                case "Employeepositions":
                    UrlResponse = $"{urlBaseOne}employeepositions";
                    break;

                //Documentos de empleado
                case "EmployeeDocument":
                    UrlResponse = $"{urlBaseOne}employeedocuments";
                    break;

                //Nominas activas
                case "Payroll":
                    UrlResponse = $"{urlBaseOne}payrolls/enabled";
                    break;

                //Nominas inactivas
                case "PayrollDisabled":
                    UrlResponse = $"{urlBaseOne}payrolls/disabled";
                    break;

                //Monedas
                case "Currencies":
                    UrlResponse = $"{urlBaseOne}currencies";
                    break;

                //Dashboard
                case "Dashboard":
                    UrlResponse = $"{urlBaseOne}dashboard";
                    break;

                //Proyectos 
                //Ajustar la URL desde la api 
                case "ProjectsEnabled":
                    UrlResponse = $"{urlBaseOne}projects/enabled";
                    break;

                //Categoría de proyectos activa
                case "ProjCategoryEnabled":
                    UrlResponse = $"{urlBaseOne}projcategories/enabled";
                    break;

                //Categoría de proyectos inactiva
                case "ProjCategoryDisabled":
                    UrlResponse = $"{urlBaseOne}projcategories/disabled";
                    break;

                //PayCycle
                case "PayCycle":
                    UrlResponse = $"{urlBaseOne}paycycle";
                    break;

                //Proceso de nómina
                case "PayrollProcess":
                    UrlResponse = $"{urlBaseOne}payrollprocess";
                    break;
                //detalle de proceso de nomina
                case "Payrollprocessdetails":
                    UrlResponse = $"{urlBaseOne}payrollprocessdetails​";
                    break;

                //Novedades empleados
                case "Payrollprocessactions":
                    UrlResponse = $"{urlBaseOne}payrollprocessactions";
                    break;

                //Prestamos de empleado
                case "Employeeloans":
                    UrlResponse = $"{urlBaseOne}employeeloans";
                    break;

                //Prestamos activos
                case "Loans":
                    UrlResponse = $"{urlBaseOne}loans/enabled";
                    break;

                //Prestamos inactivos
                case "DisabledLoands":
                    UrlResponse = $"{urlBaseOne}loans/disabled";
                    break;

                //Impuesto
                case "Taxes​Enabled":
                    UrlResponse = $"{urlBaseOne}taxes/enabled";
                    break;

                //Impuesto inactivos
                case "Taxes​Disabled":
                    UrlResponse = $"{urlBaseOne}taxes/disabled";
                    break;

                //Impuesto
                case "Taxdetails":
                    UrlResponse = $"{urlBaseOne}taxdetails";
                    break;

                //Impuestos de empleados
                case "EmployeeTax":
                    UrlResponse = $"{urlBaseOne}employeetaxes";
                    break;

                //Hora extras empleados
                case "hora":
                    UrlResponse = $"{urlBaseOne}​employeeextrahours";
                    break;

                case "Provinces":
                    UrlResponse = $"{urlBaseOne}​provinces";
                    break;
            }

            return UrlResponse;

        }
    }
}
