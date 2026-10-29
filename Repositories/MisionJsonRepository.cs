using DSW_3P_PR01.Models;
using System.Text.Json;

namespace DSW_3P_PR01.Repositories;

public class MisionJsonRepository : IMisionRepository
{
    private readonly string _ruta;
    private readonly object _bloqueo = new();
    private readonly JsonSerializerOptions _opciones = new()
    {
        WriteIndented = true
    };

    public MisionJsonRepository(IWebHostEnvironment environment)
    {
        _ruta = Path.Combine(environment.ContentRootPath, "Data", "misiones.json");

        Directory.CreateDirectory(Path.GetDirectoryName(_ruta)!);

        if (!File.Exists(_ruta))
        {
            File.WriteAllText(_ruta, "[]");
        }
    }

    private List<Mision> Leer()
    {
        var json = File.ReadAllText(_ruta);
        return JsonSerializer.Deserialize<List<Mision>>(json, _opciones)
            ?? throw new InvalidDataException("El archivo de misiones contiene un valor JSON nulo.");
    }

    private void Guardar(List<Mision> misiones)
    {
        var json = JsonSerializer.Serialize(misiones, _opciones);
        var archivoTemporal = _ruta + ".tmp";
        File.WriteAllText(archivoTemporal, json);
        File.Move(archivoTemporal, _ruta, overwrite: true);
    }

    public List<Mision> ObtenerTodas()
    {
        lock (_bloqueo)
        {
            return Leer();
        }
    }

    public Mision? ObtenerPorId(int id)
    {
        lock (_bloqueo)
        {
            return Leer().FirstOrDefault(mision => mision.Id == id);
        }
    }

    public Mision Agregar(Mision mision)
    {
        ArgumentNullException.ThrowIfNull(mision);

        lock (_bloqueo)
        {
            var misiones = Leer();
            mision.Id = misiones.Count == 0 ? 1 : checked(misiones.Max(registrada => registrada.Id) + 1);
            misiones.Add(mision);
            Guardar(misiones);
            return mision;
        }
    }

    public bool Actualizar(int id, Mision mision)
    {
        ArgumentNullException.ThrowIfNull(mision);

        lock (_bloqueo)
        {
            var misiones = Leer();
            var existente = misiones.FirstOrDefault(registrada => registrada.Id == id);

            if (existente is null)
            {
                return false;
            }

            existente.Nombre = mision.Nombre;
            existente.Dificultad = mision.Dificultad;
            existente.RecompensaXP = mision.RecompensaXP;
            existente.Completada = mision.Completada;

            Guardar(misiones);
            return true;
        }
    }

    public bool Eliminar(int id)
    {
        lock (_bloqueo)
        {
            var misiones = Leer();
            var mision = misiones.FirstOrDefault(registrada => registrada.Id == id);

            if (mision is null)
            {
                return false;
            }

            misiones.Remove(mision);
            Guardar(misiones);
            return true;
        }
    }
}
