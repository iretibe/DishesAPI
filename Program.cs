using AutoMapper;
using DishesAPI.Data;
using DishesAPI.Entities;
using DishesAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DishesContext>(o => o.UseSqlite(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//app.MapGet("/dishes", async (DishesContext dishesContext, IMapper mapper) =>
//{      
//    return mapper.Map<IEnumerable<DishDto>>(await dishesContext.Dishes.ToListAsync());
//});

//app.MapGet("/dishes", async (DishesContext dishesContext, ClaimsPrincipal claimsPrincipal, IMapper mapper, [FromQuery] string name) =>
//{
//    Console.WriteLine($"User authenticated? {claimsPrincipal.Identity?.IsAuthenticated}");

//    return TypedResults.Ok(mapper.Map<IEnumerable<DishDto>>(await dishesContext.Dishes.Where(d => name == null || d.Name.Contains(name)).ToListAsync()));
//});

app.MapGet("/dishes", async Task<Ok<IEnumerable<DishDto>>>(DishesContext dishesContext, ClaimsPrincipal claimsPrincipal, IMapper mapper, [FromQuery] string name) =>
{
    Console.WriteLine($"User authenticated? {claimsPrincipal.Identity?.IsAuthenticated}");

    return TypedResults.Ok(mapper.Map<IEnumerable<DishDto>>(await dishesContext.Dishes.Where(d => name == null || d.Name.Contains(name)).ToListAsync()));
});

//app.MapGet("/dishes/{dishId:guid}", async (DishesContext dishesContext, IMapper mapper, Guid dishId) =>
//{
//    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

//    if (dishEntity is null)
//    {
//        return Results.NotFound();
//    }

//    return TypedResults.Ok(mapper.Map<DishDto>(dishEntity));
//});

app.MapGet("/dishes/{dishId:guid}", async Task<Results<NotFound, Ok<DishDto>>> (DishesContext dishesContext, IMapper mapper, Guid dishId) =>
{
    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

    if (dishEntity is null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(mapper.Map<DishDto>(dishEntity));
});

//app.MapGet("/dishes/{dishName}", async (DishesContext dishesContext, IMapper mapper, string dishName) =>
//{
//    return mapper.Map<DishDto>(await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Name == dishName));
//});

app.MapGet("/dishes/{dishName}", async Task<Ok<DishDto>> (DishesContext dishesContext, IMapper mapper, string dishName) =>
{
    return TypedResults.Ok(mapper.Map<DishDto>(await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Name == dishName)));
});

app.MapGet("/dishes/{dishId}/ingredients", async (DishesContext dishesContext, IMapper mapper, Guid dishId) =>
{
    return mapper.Map<IEnumerable<IngredientDto>>((await dishesContext.Dishes
        .Include(d => d.Ingredients)
        .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients);
});

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<DishesContext>();

    context.Database.EnsureCreated();
    context.Database.Migrate();
}

app.Run();