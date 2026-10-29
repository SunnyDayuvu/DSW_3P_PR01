using DSW_3P_PR01.Models;
using DSW_3P_PR01.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR01.Controllers;

[ApiController]
[Route("api/misiones")]
public class MisionController : ControllerBase
{
    private readonly IMisionService _service;

    public MisionController(IMisionService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Mision>), StatusCodes.Status200OK)]
    public IActionResult ObtenerTodas()
    {
        return Ok(_service.ObtenerTodas());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorId(int id)
    {
        var mision = _service.ObtenerPorId(id);
        if (mision is null)
            return NotFound();

        return Ok(mision);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status201Created)]
    public IActionResult Agregar([FromBody] Mision mision)
    {
        var creada = _service.Agregar(mision);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Actualizar(int id, [FromBody] Mision mision)
    {
        var actualizada = _service.Actualizar(id, mision);
        if (actualizada is null)
            return NotFound();

        return Ok(actualizada);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        if (!_service.Eliminar(id))
            return NotFound();

        return NoContent();
    }
}
