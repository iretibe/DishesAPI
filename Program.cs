using DishesAPI.Data;
using DishesAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DishesContext>(o => o.UseSqlite(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseExceptionHandler(configureApplicationBuilder =>
//{
//    configureApplicationBuilder.Run(
//        async context =>
//        {
//            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            context.Response.ContentType = "text/html";
//            await context.Response.WriteAsync("An unexpected problem happened.");
//        });
//});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    //app.UseExceptionHandler(configureApplicationBuilder =>
    //{
    //    configureApplicationBuilder.Run(
    //        async context =>
    //        {
    //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            context.Response.ContentType = "text/html";
    //            await context.Response.WriteAsync("An unexpected problem happened.");
    //        });
    //});
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//var dishEnpoints = app.MapGroup("/dishes");
//var dishWithGuidIdEnpoints = dishEnpoints.MapGroup("/{dishId:guid}");
//var ingredientEnpoints = dishWithGuidIdEnpoints.MapGroup("/ingredients");

//app.MapGet("/dishes", async (DishesContext dishesContext, IMapper mapper) =>
//{      
//    return mapper.Map<IEnumerable<DishDto>>(await dishesContext.Dishes.ToListAsync());
//});

//app.MapGet("/dishes", async (DishesContext dishesContext, ClaimsPrincipal claimsPrincipal, IMapper mapper, [FromQuery] string name) =>
//{
//    Console.WriteLine($"User authenticated? {claimsPrincipal.Identity?.IsAuthenticated}");

//    return TypedResults.Ok(mapper.Map<IEnumerable<DishDto>>(await dishesContext.Dishes.Where(d => name == null || d.Name.Contains(name)).ToListAsync()));
//});

//app.MapGet("/dishes", async Task<Ok<IEnumerable<DishDto>>>(DishesContext dishesContext, ClaimsPrincipal claimsPrincipal, IMapper mapper, [FromQuery] string name) =>
//{
//    Console.WriteLine($"User authenticated? {claimsPrincipal.Identity?.IsAuthenticated}");

//    return TypedResults.Ok(mapper.Map<IEnumerable<DishDto>>(await dishesContext.Dishes.Where(d => name == null || d.Name.Contains(name)).ToListAsync()));
//});

//dishEnpoints.MapGet("", async Task<Ok<IEnumerable<DishDto>>> (DishesContext dishesContext, ClaimsPrincipal claimsPrincipal, IMapper mapper, [FromQuery] string name) =>
//{
//    Console.WriteLine($"User authenticated? {claimsPrincipal.Identity?.IsAuthenticated}");

//    return TypedResults.Ok(mapper.Map<IEnumerable<DishDto>>(await dishesContext.Dishes.Where(d => name == null || d.Name.Contains(name)).ToListAsync()));
//});

//app.MapGet("/dishes/{dishId:guid}", async (DishesContext dishesContext, IMapper mapper, Guid dishId) =>
//{
//    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

//    if (dishEntity is null)
//    {
//        return Results.NotFound();
//    }

//    return TypedResults.Ok(mapper.Map<DishDto>(dishEntity));
//});

//app.MapGet("/dishes/{dishId:guid}", async Task<Results<NotFound, Ok<DishDto>>> (DishesContext dishesContext, IMapper mapper, Guid dishId) =>
//{
//    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

//    if (dishEntity is null)
//    {
//        return TypedResults.NotFound();
//    }

//    return TypedResults.Ok(mapper.Map<DishDto>(dishEntity));
//}).WithName("GetDish");

//dishWithGuidIdEnpoints.MapGet("", async Task<Results<NotFound, Ok<DishDto>>> (DishesContext dishesContext, IMapper mapper, Guid dishId) =>
//{
//    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

//    if (dishEntity is null)
//    {
//        return TypedResults.NotFound();
//    }

//    return TypedResults.Ok(mapper.Map<DishDto>(dishEntity));
//}).WithName("GetDish");

//app.MapGet("/dishes/{dishName}", async (DishesContext dishesContext, IMapper mapper, string dishName) =>
//{
//    return mapper.Map<DishDto>(await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Name == dishName));
//});

//app.MapGet("/dishes/{dishName}", async Task<Ok<DishDto>> (DishesContext dishesContext, IMapper mapper, string dishName) =>
//{
//    return TypedResults.Ok(mapper.Map<DishDto>(await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Name == dishName)));
//});

//dishEnpoints.MapGet("/{dishName}", async Task<Ok<DishDto>> (DishesContext dishesContext, IMapper mapper, string dishName) =>
//{
//    return TypedResults.Ok(mapper.Map<DishDto>(await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Name == dishName)));
//});

//app.MapGet("/dishes/{dishId}/ingredients", async (DishesContext dishesContext, IMapper mapper, Guid dishId) =>
//{
//    return mapper.Map<IEnumerable<IngredientDto>>((await dishesContext.Dishes
//        .Include(d => d.Ingredients)
//        .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients);
//});

//app.MapGet("/dishes/{dishId}/ingredients", async Task<Results<NotFound, Ok<IEnumerable<IngredientDto>>>> (DishesContext dishesContext,  IMapper mapper,  Guid dishId) =>
//{
//    var dishEntity = await dishesContext.Dishes .FirstOrDefaultAsync(d => d.Id == dishId);
//    if (dishEntity == null)
//    {
//        return TypedResults.NotFound();
//    }

//    return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDto>>((await dishesContext.Dishes
//        .Include(d => d.Ingredients)
//        .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients));
//});

//ingredientEnpoints.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<IngredientDto>>>> (DishesContext dishesContext, IMapper mapper, Guid dishId) =>
//{
//    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
//    if (dishEntity == null)
//    {
//        return TypedResults.NotFound();
//    }

//    return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDto>>((await dishesContext.Dishes
//        .Include(d => d.Ingredients)
//        .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients));
//});


//app.MapPost("/dishes", async Task<Created<DishDto>> (DishesContext dishesContext, IMapper mapper, DishForCreationDto dishForCreationDto, LinkGenerator linkGenerator, HttpContext httpContext) =>
//{
//    var dishEntity = mapper.Map<Dish>(dishForCreationDto);
//    dishesContext.Add(dishEntity);
//    await dishesContext.SaveChangesAsync();

//    var dishToReturn = mapper.Map<DishDto>(dishEntity);

//    var linkToDish = linkGenerator.GetUriByName(httpContext, "GetDish", new { dishId = dishToReturn.Id });

//    //return TypedResults.Ok(dishToReturn);
//    //return TypedResults.Created($"https://localhost:7257/dishes/{dishToReturn.Id}", dishToReturn);

//    return TypedResults.Created(linkToDish, dishToReturn);
//});

//app.MapPost("/dishes", async Task<CreatedAtRoute<DishDto>> (DishesContext dishesContext, IMapper mapper, DishForCreationDto dishForCreationDto) =>
//{
//    var dishEntity = mapper.Map<Dish>(dishForCreationDto);
//    dishesContext.Add(dishEntity);
//    await dishesContext.SaveChangesAsync();

//    var dishToReturn = mapper.Map<DishDto>(dishEntity);

//    //return TypedResults.Ok(dishToReturn);
//    //return TypedResults.Created($"https://localhost:7257/dishes/{dishToReturn.Id}", dishToReturn);

//    return TypedResults.CreatedAtRoute(dishToReturn, "GetDish", new { dishId = dishToReturn.Id});
//});

//dishEnpoints.MapPost("", async Task<CreatedAtRoute<DishDto>> (DishesContext dishesContext, IMapper mapper, DishForCreationDto dishForCreationDto) =>
//{
//    var dishEntity = mapper.Map<Dish>(dishForCreationDto);
//    dishesContext.Add(dishEntity);
//    await dishesContext.SaveChangesAsync();

//    var dishToReturn = mapper.Map<DishDto>(dishEntity);

//    //return TypedResults.Ok(dishToReturn);
//    //return TypedResults.Created($"https://localhost:7257/dishes/{dishToReturn.Id}", dishToReturn);

//    return TypedResults.CreatedAtRoute(dishToReturn, "GetDish", new { dishId = dishToReturn.Id });
//});

//app.MapPut("/dishes/{dishId:guid}", async Task<Results<NotFound, NoContent>> (DishesContext dishesContext, IMapper mapper, Guid dishId, DishForUpdateDto dishForUpdateDto) =>
//{
//    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id.Equals(dishId));

//    if (dishEntity is null)
//    {
//        return TypedResults.NotFound();
//    }

//    mapper.Map(dishForUpdateDto, dishEntity);

//    await dishesContext.SaveChangesAsync();

//    return TypedResults.NoContent();
//});

//dishWithGuidIdEnpoints.MapPut("", async Task<Results<NotFound, NoContent>> (DishesContext dishesContext, IMapper mapper, Guid dishId, DishForUpdateDto dishForUpdateDto) =>
//{
//    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id.Equals(dishId));

//    if (dishEntity is null)
//    {
//        return TypedResults.NotFound();
//    }

//    mapper.Map(dishForUpdateDto, dishEntity);

//    await dishesContext.SaveChangesAsync();

//    return TypedResults.NoContent();
//});

//app.MapDelete("/dishes/{dishId:guid}", async Task<Results<NotFound, NoContent>> (DishesContext dishesContext, Guid dishId) =>
//{
//    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id.Equals(dishId));

//    if (dishEntity is null)
//    {
//        return TypedResults.NotFound();
//    }

//    dishesContext.Dishes.Remove(dishEntity);

//    await dishesContext.SaveChangesAsync();

//    return TypedResults.NoContent();
//});

//dishWithGuidIdEnpoints.MapDelete("", async Task<Results<NotFound, NoContent>> (DishesContext dishesContext, Guid dishId) =>
//{
//    var dishEntity = await dishesContext.Dishes.FirstOrDefaultAsync(d => d.Id.Equals(dishId));

//    if (dishEntity is null)
//    {
//        return TypedResults.NotFound();
//    }

//    dishesContext.Dishes.Remove(dishEntity);

//    await dishesContext.SaveChangesAsync();

//    return TypedResults.NoContent();
//});

app.RegisterDishesEndpoints();
app.RegisterIngredientsEndpoints();

//using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
//{
//    var context = serviceScope.ServiceProvider.GetRequiredService<DishesContext>();

//    context.Database.EnsureCreated();
//    context.Database.Migrate();
//}

app.Run();