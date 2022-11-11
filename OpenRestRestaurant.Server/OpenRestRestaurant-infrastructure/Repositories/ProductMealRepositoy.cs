using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;

namespace OpenRestRestaurant_infrastructure.Repositories
{
    public class ProductMealRepositoy : BaseRepository<ProductMeal>, IProductMealRepository
    {
        public ProductMealRepositoy(OpenRestRestaurantDbContext dbContext) : base(dbContext)
        {

        }
    }
}
