using DTOs;
using Infrastracture.Application.Configuration;
using Infrastracture.Application.DTOs;
using Infrastracture.Infrastracture;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.ConfigurePresistanceApp(builder.Configuration);
builder.Services.ConfigureApplicationApp();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<AuthCheckConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://ryqfbrei:ZzSKvw_5rVinY_QLFwQ3evnA2EJgogn4@kebnekaise.lmq.cloudamqp.com/ryqfbrei");
        cfg.ReceiveEndpoint("check-auth-queue", x =>
        {
            x.ConfigureConsumer<AuthCheckConsumer>(context);
            x.Bind("exchange-name");
        });

    });

});

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "v1");
    });
}

app.UseHttpsRedirection();

app.Run();