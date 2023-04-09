using AutoMapper;
using RussianMunchkin.Server.Core;

namespace RussianMunchkin.Server.Mapper;

public class MapperInstance
{
    private readonly AutoMapper.Mapper _mapper;
    public static AutoMapper.Mapper Mapper { get; private set; }

    public MapperInstance()
    {
        _mapper = new AutoMapper.Mapper(new MapperConfiguration(cfg 
            => cfg.AddProfile(new AutoMappingProfile())));
        Mapper = _mapper;
    }
}