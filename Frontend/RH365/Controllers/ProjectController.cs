// ============================================================================
// Archivo: ProjectController.cs
// Proyecto: RH365 (Front-End MVC .NET 8)
// Ruta: Controllers/ProjectController.cs
// Descripción:
//   - LP_Projects: muestra la ListPage.
//   - NewEdit: entrega la vista NewEdit_Project.cshtml (nueva o edición).
// Notas:
//   - Obtiene DataareaID y UserRefRecID desde IUserContext (sin hardcode).
//   - Para edición, la vista puede venir vacía y el JS consultará a la WebAPI,
//     o puedes pasar el objeto Project en Model.Project si luego agregas un servicio.
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using RH365.Infrastructure.Services;

namespace RH365.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IUserContext _user;

        public ProjectController(IUserContext user)
        {
            _user = user;
        }

        /// <summary>ListPage de Proyectos.</summary>
        [HttpGet]
        public IActionResult LP_Projects()
        {
            var vm = new
            {
                DataareaID = _user.DataareaID,
                UserRefRecID = _user.UserRefRecID
            };

            return View("~/Views/Project/LP_Projects.cshtml", vm);
        }

        /// <summary>
        /// Formulario Nuevo/Editar Proyecto.
        /// Si recId es null o 0 => Nuevo. Si &gt;0 => Editar (el JS puede cargar datos).
        /// </summary>
        [HttpGet]
        public IActionResult NewEdit(long? recId = null)
        {
            var vm = new
            {
                DataareaID = _user.DataareaID,
                UserRefRecID = _user.UserRefRecID,
                // Opcional: incluir solo RecID y que el JS haga GET a /api/Projects/{id}
                Project = (object?)null,
                RecID = recId ?? 0L
            };

            return View("~/Views/Project/NewEdit_Project.cshtml", vm);
        }
    }
}
