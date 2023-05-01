using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using StockProduct.Api.Mapper;
using StockProduct.Application.Configuration;
using StockProduct.Application.Dtos;
using StockProduct.Application.Interfaces;
using StockProduct.Application.Policies;
using StockProduct.Application.Services;
using StockProduct.Application.Validator;
using StockProduct.Infrastructure.Message.Broker;
using StockProduct.Infrastructure.Message.Broker.Consumer;
using StockProduct.Infrastructure.Message.Broker.Producer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<StockProductOutput>, StockProductValidator>();

var applicationSettings = builder.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>();
builder.Services.AddSingleton<IApplicationSettings>(applicationSettings!);

builder.Services.AddScoped<IProducerService, ProducerService>();

builder.Services.AddScoped<IConsumerService, ConsumerService>();

builder.Services.AddScoped<IKafkaBroker, KafkaBroker>();

builder.Services.AddScoped<IPolicyFactory, PolicyFactory>();

builder.Services.AddScoped<IDeliveryService, DeliveryService>();

builder.Services.AddHttpClient("", config =>
{
    config.BaseAddress = new Uri(applicationSettings!.Kafka.Destination.Host);
    config.Timeout = TimeSpan.FromMinutes(5);
    config.DefaultRequestHeaders.Add("Accept", "application/json");
}).SetHandlerLifetime(TimeSpan.FromMinutes(5));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/stock-product/{topic}", async ([FromRoute(Name = "topic")] string topic, IKafkaBroker _broker, IValidator<StockProductInput> Validator, StockProductInput product) =>
{
    await _broker.Produce(product.Map(topic));

    return Results.Ok();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
