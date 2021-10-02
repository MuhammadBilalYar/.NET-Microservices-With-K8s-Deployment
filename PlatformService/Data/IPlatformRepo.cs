using PlatformService.Models;
using System.Collections;
using System.Collections.Generic;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatform(int id);
        void CreatePlatform(Platform platform);
    }
}
