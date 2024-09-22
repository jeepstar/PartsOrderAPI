using PartsOrderAPI.DTO;
using PartsOrderAPI.Models;

namespace PartsOrderAPI.Services
{
    public interface IPartService
    {
        IEnumerable<Part> GetParts();
        Part CreatePart(PartDto partDto);
    }
}
