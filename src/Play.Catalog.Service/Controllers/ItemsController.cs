using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers 
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        // Dependency Injection - IItemsRepository and we don't create an instance so we dont do ->  = new()
        private readonly IRepository<Item> itemsRepository;

        // It is time to introduce a constructor for this controller and here we inject the ItemRepository dependency
        public ItemsController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync() 
        {  
            // Use our extension method to turn item entity into itemDto.
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
            return items;
        }


        // GET  /items/{id}      <- The idea from parameter
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            } 
            return item.AsDto(); 
        }

        
        [HttpPost]
        // An action result is what a controller action returns in response to a browser request.
        // It enables you to return a type deriving from ActionResult or return a specific type.
        // As we can see below, we can also specifically have type ItemDto as well.
        // For this request we are going to receive a CreateItemDto contract (parameter is CreateItemDto).
        // This way we enforce whoever calls this API has to follow our contract for Creating items as shown in the Dtos.cs file.
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto) 
        {
            //Create item entity from the properties of CreateItemDto
            var item = new Item{
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetItemByIdAsync), new {id = item.Id}, item);
            // CreatedAtAction means it creates a CreatedAtActionResult object that produces a Status201Created response.
            // CreatedAtAction(nameof(GetItemById)) indicates item has been created and you can find it at the following route. 
        }


        // This request is for updating items. We have to specify what part of the route is used for id parameter as shown below.
        [HttpPut("{id}")]
        // We are using IActionResult because we don't need a specific type to return.
        // Usually with PUT (update) we don't return anything at all therefore we use IActionResult instead of ActionResult.
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            //First find item in data repository
            var existingItem = await itemsRepository.GetAsync(id);

            if(existingItem == null) 
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await itemsRepository.GetAsync(id);
            if(existingItem == null) 
            {
                return NoContent();
                
            }
            await itemsRepository.RemoveAsync(existingItem.Id);
            return NoContent(); 
        }

        

    }
}