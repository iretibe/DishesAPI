using AutoMapper;
using DishesAPI.Data;
using DishesAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DishesAPI.EndpointHandlers
{
    public class IngredientsHandlers
    {
        public static async Task<Results<NotFound, Ok<IEnumerable<IngredientDto>>>> GetIngredientsAsync(DishesContext dishesContext, IMapper mapper, Guid dishId)
        {
            var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            if (dishEntity == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDto>>((await dishesContext.Dishes
                .Include(d => d.Ingredients)
                .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients));
        }
    }
}
