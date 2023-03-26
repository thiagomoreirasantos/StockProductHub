using FluentValidation;
using StockProduct.Api.Validator;
using StockProduct.Application.Configuration;
using StockProduct.Application.Dtos;
using StockProduct.Application.Interfaces;
using StockProduct.Application.Policies;
using StockProduct.Infrastructure.Message.Broker;
using StockProduct.Infrastructure.Message.Broker.Consumer;
using StockProduct.Infrastructure.Message.Broker.Producer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<StockProductData>, StockProductValidator>();

var applicationSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>();
builder.Services.AddSingleton<IApplicationSettings>(applicationSettings!);

builder.Services.AddScoped<IProducerService, ProducerService>();

builder.Services.AddScoped<IConsumerService, ConsumerService>();

builder.Services.AddScoped<IKafkaBroker, KafkaBroker>();

builder.Services.AddScoped<IPolicyFactory, PolicyFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/stock-product/{topic}", async (IKafkaBroker _broker, IValidator<StockProductData> Validator, StockProductData product) =>
{
    var validationResult = await Validator.ValidateAsync(product);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());

    await _broker.Produce(product);

    return Results.Ok();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
