using PartsOrderAPI.Data;
using PartsOrderAPI.DTO;
using PartsOrderAPI.Models;
namespace PartsOrderAPI.Services
{
    public class PartService : IPartService
    {
        public IEnumerable<Part> GetParts()
        {
            return InMemoryDataStore.Parts;
        }

        public Part CreatePart(PartDto partDto)
        {
            var part = new Part
            {
                Id = InMemoryDataStore.Parts.Max(p => p.Id) + 1,
                Description = partDto.Description,
                Price = partDto.Price,
                Quantity = partDto.Quantity
            };
            InMemoryDataStore.Parts.Add(part);
            return part;
        }
    }
}
