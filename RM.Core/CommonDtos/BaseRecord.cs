using Mapster;

namespace RM.Core.CommonDtos;

public abstract record BaseRecord<TRecord, TEntity> : IRegister
    where TRecord : class, new()
    where TEntity : class, new()
{

    public TEntity ToEntity()
    {
        return this.Adapt<TEntity>();
    }

    public TEntity ToEntity(TEntity entity)
    {
        return (this as TRecord).Adapt(entity);
    }

    public static TRecord FromEntity(TEntity entity)
    {
        return entity.Adapt<TRecord>();
    }


    private TypeAdapterConfig Config { get; set; }

    public virtual void AddCustomMappings()
    {

    }


    protected TypeAdapterSetter<TRecord, TEntity> SetCustomMappings()

        => Config.ForType<TRecord, TEntity>();

    protected TypeAdapterSetter<TEntity, TRecord> SetCustomMappingsInverse()

        => Config.ForType<TEntity, TRecord>();

    public void Register(TypeAdapterConfig config)
    {
        Config = config;
        AddCustomMappings();
    }
}
