using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos {
    //The ItemDto is the DTO that will be returned to the client (user) from a GET operation
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);

    //The CreateItemDto is the DTO for creating an item.
    //Notice this DTO doesn't have a Id nor a Created Date. This is because both will be automatically generated WITHIN the serivce so doesn't need to be provided to client.
    public record CreateItemDto([Required] string Name, string Description, [Range(0,1000)] decimal Price);

    //This UpdateItemDto is the DTO for updating an item.
    public record UpdateItemDto([Required] string Name, string Description, [Range(0,1000)] decimal Price);
} 