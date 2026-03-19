// IMap.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository. : warning

//TODO: Document this.
namespace Kernel.Data.Mapping
{
    public interface IMap<in TFrom, out TTo>
    {
        static abstract TTo MapFrom(TFrom entity);
    }

}
