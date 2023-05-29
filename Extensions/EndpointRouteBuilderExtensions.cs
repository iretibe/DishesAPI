﻿using DishesAPI.EndpointFilters;
using DishesAPI.EndpointHandlers;

namespace DishesAPI.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterDishesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var dishesEndpoints = endpointRouteBuilder.MapGroup("/dishes");

            var dishWithGuidIdEndpoints = dishesEndpoints.MapGroup("/{dishId:guid}");

            var dishWithGuidIdEndpointsAndLockFilters = endpointRouteBuilder.MapGroup("/dishes/{dishId:guid}")
                .AddEndpointFilter(new DishIsLockedFilter(new("fd630a57-2352-4731-b25c-db9cc7601b16")))
                .AddEndpointFilter(new DishIsLockedFilter(new("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06")));

            dishesEndpoints.MapGet("", DishesHandlers.GetDishesAsync);

            dishWithGuidIdEndpoints.MapGet("", DishesHandlers.GetDishByIdAsync).WithName("GetDish");

            dishesEndpoints.MapGet("/{dishName}", DishesHandlers.GetDishByNameAsync);

            //dishesEndpoints.MapPost("", DishesHandlers.CreateDishAsync);
            dishesEndpoints.MapPost("", DishesHandlers.CreateDishAsync).AddEndpointFilter<ValidateAnnotationsFilter>();

            dishWithGuidIdEndpoints.MapPut("", DishesHandlers.UpdateDishAsync).AddEndpointFilter(async (context, next) =>
            {
                var dishId = context.GetArgument<Guid>(2);

                var valueId = new Guid("429AA8DB-3004-480A-8AF3-036D172A57E1");

                if (dishId == valueId)
                {
                    return TypedResults.Problem(new()
                    {
                        Status = 400,
                        Title = "Dish is perfect and cannot be changed",
                        Detail = "You cannot update perfection"
                    });
                }

                //Invoke the next filter
                var result = await next.Invoke(context);

                return result;
            });

            //dishWithGuidIdEndpoints.MapDelete("", DishesHandlers.DeleteDishAsync);
            dishWithGuidIdEndpointsAndLockFilters.MapDelete("", DishesHandlers.DeleteDishAsync).AddEndpointFilter<LogNotFoundResponseFilter>();
        }

        public static void RegisterIngredientsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var ingredientsEndpoints = endpointRouteBuilder.MapGroup("/dishes/{dishId:guid}/ingredients");

            ingredientsEndpoints.MapGet("", IngredientsHandlers.GetIngredientsAsync);

            ingredientsEndpoints.MapPost("", () =>
            {
                throw new NotImplementedException();
            });
        }
    }
}
