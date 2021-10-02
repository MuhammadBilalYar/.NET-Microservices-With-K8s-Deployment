using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataService.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            this._repo = repo;
            this._mapper = mapper;
            this._commandDataClient = commandDataClient;
        }

        [HttpGet]
        public IEnumerable<PlatformReadDto> GetPlaforms()
        {
            var platformItems = this._repo.GetAllPlatforms();
            return this._mapper.Map<IEnumerable<PlatformReadDto>>(platformItems);
        }

        [HttpGet("{id}", Name = "GetPlaformById")]
        public ActionResult<PlatformReadDto> GetPlaformById(int id)
        {
            var platformItems = this._repo.GetPlatform(id);

            if (null == platformItems)
                return NotFound();

            return this._mapper.Map<PlatformReadDto>(platformItems);
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlaform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = this._mapper.Map<Platform>(platformCreateDto);
            this._repo.CreatePlatform(platformModel);
            this._repo.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await this._commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Cound not send synchronously:{ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlaformById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}
