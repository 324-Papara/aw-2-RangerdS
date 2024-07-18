using Microsoft.AspNetCore.Mvc;
using Pa.Api.Extensions;
using Pa.Base.Response;
using Pa.Data.Domain;
using Pa.Data.UnitOfWork;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace Pa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactoriesController// : BaseController<Factory, long>
    {
        private readonly IUnitOfWork unitOfWork;
        public FactoriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/Factories
        [HttpGet]
        public async Task<ApiResponse<List<Factory>>> Get()
        {
            var entityList = await unitOfWork.FactoryRepository.GetAll();
            if (!entityList.Any())
            {
                return new ApiResponse<List<Factory>>(null, false, "Entity not found");
            }
            return new ApiResponse<List<Factory>>(entityList);
        }

        // GET: api/Factories/GetWithLocation?withLocation=true
        [HttpGet("GetWithLocation")]
        public async Task<ApiResponse<List<Factory>>> GetWithLocation(bool? withLocation = false)
        {
            var entityList = await unitOfWork.FactoryRepository.GetAll(
                null,
                withLocation.HasValue && withLocation.Value ? q => q.Include(x => x.FactoryLocations) : null);

            if (!entityList.Any())
            {
                return new ApiResponse<List<Factory>>(null, false, "Entity not found");
            }
            return new ApiResponse<List<Factory>>(entityList);
        }

        // GET: api/Factories/GetFilterByFactoryName?factoryName=string
        [HttpGet("GetFilterByFactoryName")]
        public async Task<ApiResponse<List<Factory>>> GetFilterByFactoryName(string? factoryName)
        {
            Expression<Func<Factory, bool>> filter = x => true;

            if (!string.IsNullOrEmpty(factoryName))
            {
                // Filter: FactoryName
                filter = filter.AndAlso(x => x.FactoryName.ToLower().Contains(factoryName.ToLower()));
            }
            
            var entityList = await unitOfWork.FactoryRepository.GetAll(filter);

            if (!entityList.Any())
            {
                return new ApiResponse<List<Factory>>(null, false, "Entity not found");
            }

            return new ApiResponse<List<Factory>>(entityList);
        }

        // GET: api/Factories/GetFilterByCapacity?capacity=135
        [HttpGet("GetFilterByCapacity")]
        public async Task<ApiResponse<List<Factory>>> GetFilterByCapacity(int? capacity)
        {
            Expression<Func<Factory, bool>> filter = x => true;

            if (capacity.HasValue)
            {
                // Filter: Capacity
                filter = filter.AndAlso(x => x.Capacity == capacity);
            }
            
            var entityList = await unitOfWork.FactoryRepository.GetAll(filter);

            if (!entityList.Any())
            {
                return new ApiResponse<List<Factory>>(null, false, "Entity not found");
            }

            return new ApiResponse<List<Factory>>(entityList);
        }

        // GET: api/Factories/5
        [HttpGet("{id}")]
        public async Task<ApiResponse<Factory>> Get(long id)
        {
            var entity = await unitOfWork.FactoryRepository.GetById(id);
            if (entity == null)
            {
                return new ApiResponse<Factory>(null, false, "Entity not found");
            }
            return new ApiResponse<Factory>(entity);
        }

        // POST: api/Factories
        [HttpPost]
        public async Task<ApiResponse<Factory>> Post([FromBody] Factory entity)
        {
            await unitOfWork.FactoryRepository.Insert(entity);
            await unitOfWork.Complete();
            return new ApiResponse<Factory>(null, true, "Entity successfully created");
        }

        // PUT: api/Factories/5
        [HttpPut("{id}")]
        public async Task<ApiResponse<Factory>> Put(long id, [FromBody] Factory entity)
        {
            await unitOfWork.FactoryRepository.Update(id, entity);
            await unitOfWork.Complete();
            return new ApiResponse<Factory>(null, true, "Entity successfully updated");
        }

        // DELETE: api/Factories/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse<Factory>> Delete(long id)
        {
            await unitOfWork.FactoryRepository.Delete(id);
            await unitOfWork.Complete();
            return new ApiResponse<Factory>(null, true, "Entity successfully deleted");
        }
    }
}
