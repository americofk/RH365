// ============================================================================
// Archivo: PositionRequirementsController.cs
// Proyecto: RH365.API
// Ruta: RH365.API/Controllers/PositionRequirementsController.cs
// Descripción: API REST para gestión de requisitos de posición
// Estándar: ISO 27001 - Control A.12.1.1 (Procedimientos operacionales documentados)
//           Control A.12.4.1 (Registro de eventos - logging)
// ============================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RH365.Core.Domain.Models.PositionRequirement;
using RH365.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RH365.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PositionRequirementsController : ControllerBase
    {
        private readonly IPositionRequirementService _service;
        private readonly ILogger<PositionRequirementsController> _logger;

        public PositionRequirementsController(
            IPositionRequirementService service,
            ILogger<PositionRequirementsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los requisitos (paginado)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PositionRequirementListResponse>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 100)
        {
            try
            {
                var result = await _service.GetAllAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener requisitos de posición");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener requisito por RecID
        /// </summary>
        [HttpGet("{recId}")]
        public async Task<ActionResult<PositionRequirementResponse>> GetById(long recId)
        {
            try
            {
                var requirement = await _service.GetByIdAsync(recId);

                if (requirement == null)
                {
                    return NotFound(new { message = $"Requisito {recId} no encontrado" });
                }

                return Ok(requirement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener requisito {recId}");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener requisitos de una posición específica
        /// ENDPOINT CRÍTICO: /api/PositionRequirements/position/{positionRefRecID}
        /// </summary>
        [HttpGet("position/{positionRefRecID}")]
        public async Task<ActionResult<List<PositionRequirementResponse>>> GetByPosition(long positionRefRecID)
        {
            try
            {
                var requirements = await _service.GetByPositionAsync(positionRefRecID);
                return Ok(requirements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener requisitos de la posición {positionRefRecID}");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crear nuevo requisito
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PositionRequirementResponse>> Create(
            [FromBody] CreatePositionRequirementRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var requirement = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { recId = requirement.RecID }, requirement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear requisito de posición");
                return StatusCode(500, new { message = "Error al crear el requisito" });
            }
        }

        /// <summary>
        /// Actualizar requisito existente
        /// </summary>
        [HttpPut("{recId}")]
        public async Task<ActionResult<PositionRequirementResponse>> Update(
            long recId,
            [FromBody] UpdatePositionRequirementRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var requirement = await _service.UpdateAsync(recId, request);

                if (requirement == null)
                {
                    return NotFound(new { message = $"Requisito {recId} no encontrado" });
                }

                return Ok(requirement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar requisito {recId}");
                return StatusCode(500, new { message = "Error al actualizar el requisito" });
            }
        }

        /// <summary>
        /// Eliminar requisito
        /// </summary>
        [HttpDelete("{recId}")]
        public async Task<ActionResult> Delete(long recId)
        {
            try
            {
                var success = await _service.DeleteAsync(recId);

                if (!success)
                {
                    return NotFound(new { message = $"Requisito {recId} no encontrado" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar requisito {recId}");
                return StatusCode(500, new { message = "Error al eliminar el requisito" });
            }
        }
    }
}