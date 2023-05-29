using AutoMapper;
using DishesAPI.Data;
using DishesAPI.Entities;
using DishesAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DishesAPI.EndpointHandlers
{
    public static class DishesHandlers
    {
        public static async Task<Ok<IEnumerable<DishDto>>> GetDishesAsync(DishesContext dishesContext, ClaimsPrincipal claimsPrincipal, IMapper mapper, string name, ILogger<DishDto> logger)
        {
            Console.WriteLine($"User authenticated? {claimsPrincipal.Identity?.IsAuthenticated}");

            logger.LogInformation("Getting the dishes...");

            return TypedResults.Ok(mapper.Map<IEnumerable<DishDto>>(await dishesContext.Dishes.Where(d => name == null || d.Name.Contains(name)).ToListAsync()));
        }

        public static async Task<Results<NotFound, Ok<DishDto>>> GetDishByIdAsync(DishesContext dishesContext, IMapper mapper, Guid dishId)
        {
            var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            
            if (dishEntity == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(mapper.Map<DishDto>(dishEntity));
        }

        public static async Task<Ok<DishDto>> GetDishByNameAsync(DishesContext dishesContext, IMapper mapper, string dishName)
        {
            return TypedResults.Ok(mapper.Map<DishDto>(await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Name == dishName)));
        }

        public static async Task<CreatedAtRoute<DishDto>> CreateDishAsync(DishesContext dishesContext, IMapper mapper, DishForCreationDto dishForCreationDto)
        {
            var dishEntity = mapper.Map<Dish>(dishForCreationDto);
            
            dishesContext.Add(dishEntity);
            
            await dishesContext.SaveChangesAsync();

            var dishToReturn = mapper.Map<DishDto>(dishEntity);

            return TypedResults.CreatedAtRoute(
                dishToReturn,
                "GetDish",
                new
                {
                    dishId = dishToReturn.Id
                });
        }

        public static async Task<Results<NotFound, NoContent>> UpdateDishAsync(DishesContext dishesContext, IMapper mapper, Guid dishId, DishForUpdateDto dishForUpdateDto)
        {
            var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            
            if (dishEntity == null)
            {
                return TypedResults.NotFound();
            }

            mapper.Map(dishForUpdateDto, dishEntity);

            await dishesContext.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<Results<NotFound, NoContent>> DeleteDishAsync(DishesContext dishesContext, Guid dishId)
        {
            var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
            
            if (dishEntity == null)
            {
                return TypedResults.NotFound();
            }

            dishesContext.Dishes.Remove(dishEntity);

            await dishesContext.SaveChangesAsync();

            return TypedResults.NoContent();
        }
    }
}
