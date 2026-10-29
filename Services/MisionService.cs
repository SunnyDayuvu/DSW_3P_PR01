using DSW_3P_PR01.Models;
using DSW_3P_PR01.Repositories;

namespace DSW_3P_PR01.Services;

public class MisionService : IMisionService
{
    private readonly IMisionRepository _repository;

    public MisionService(IMisionRepository repository)
    {
        _repository = repository;
    }

    public List<Mision> ObtenerTodas()
    {
        return _repository.ObtenerTodas();
    }

    public Mision? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Mision Agregar(Mision mision)
    {
        ArgumentNullException.ThrowIfNull(mision);
        mision.Completada = false;
        return _repository.Agregar(mision);
    }

    public Mision? Actualizar(int id, Mision mision)
    {
        ArgumentNullException.ThrowIfNull(mision);
        mision.Id = id;
        return _repository.Actualizar(id, mision) ? mision : null;
    }

    public bool Eliminar(int id)
    {
        return _repository.Eliminar(id);
    }
}
