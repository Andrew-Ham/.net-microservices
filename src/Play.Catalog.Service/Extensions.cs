using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service 
{
    // All extension methods should be static!
    public static class Extensions 
    {
        // Purpose of method below is to turn the item entity into an ItemDto.
        public static ItemDto AsDto(this Item item) 
        {
            
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}