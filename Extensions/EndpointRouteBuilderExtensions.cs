﻿using DishesAPI.EndpointFilters;
using DishesAPI.EndpointHandlers;

namespace DishesAPI.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterDishesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var dishesEndpoints = endpointRouteBuilder.MapGroup("/dishes").RequireAuthorization();

            var dishWithGuidIdEndpoints = dishesEndpoints.MapGroup("/{dishId:guid}");

            var dishWithGuidIdEndpointsAndLockFilters = endpointRouteBuilder.MapGroup("/dishes/{dishId:guid}")
                .RequireAuthorization("RequireAdminFromBelgium")
                .AddEndpointFilter(new DishIsLockedFilter(new("fd630a57-2352-4731-b25c-db9cc7601b16")))
                .AddEndpointFilter(new DishIsLockedFilter(new("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06")));

            dishesEndpoints.MapGet("", DishesHandlers.GetDishesAsync);

            dishWithGuidIdEndpoints.MapGet("", DishesHandlers.GetDishByIdAsync).WithName("GetDish");

            dishesEndpoints.MapGet("/{dishName}", DishesHandlers.GetDishByNameAsync).AllowAnonymous();

            dishesEndpoints.MapPost("", DishesHandlers.CreateDishAsync).RequireAuthorization("RequireAdminFromBelgium").AddEndpointFilter<ValidateAnnotationsFilter>();

            dishWithGuidIdEndpointsAndLockFilters.MapPut("", DishesHandlers.UpdateDishAsync);

            dishWithGuidIdEndpointsAndLockFilters.MapDelete("", DishesHandlers.DeleteDishAsync).AddEndpointFilter<LogNotFoundResponseFilter>();
        }

        public static void RegisterIngredientsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var ingredientsEndpoints = endpointRouteBuilder.MapGroup("/dishes/{dishId:guid}/ingredients").RequireAuthorization();

            ingredientsEndpoints.MapGet("", IngredientsHandlers.GetIngredientsAsync);

            ingredientsEndpoints.MapPost("", () =>
            {
                throw new NotImplementedException();
            });
        }
    }
}
