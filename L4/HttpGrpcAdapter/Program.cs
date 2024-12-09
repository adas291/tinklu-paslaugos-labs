using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
  options.ListenLocalhost(6001);
});

var grpcChannel = GrpcChannel.ForAddress("http://localhost:5000");
var roomclient = new Room.RoomClient(grpcChannel);
builder.Services.AddSingleton(roomclient);

var app = builder.Build();

var root = app.MapGroup("/room");

root.MapPost("/canHeat", (Room.RoomClient roomClient) =>
{
  var result = roomClient.CanHeat(new Empty());
  return Results.Ok(result.Value); // Assuming Value is a bool field in the response
});


root.MapPost("/canCool", (Room.RoomClient roomClient) =>
{
  var result = roomClient.CanCool(new Empty());
  return Results.Ok(result.Value); // Assuming Value is a bool field in the response
});


root.MapPost("/heatRoom", async (float temperature, Room.RoomClient roomClient) =>
{
  var result = await roomClient.HeatAsync(new Temperature { Value = temperature });
  return Results.Ok(result.Value);

});


root.MapPost("/coolRoom", async (float temperature, Room.RoomClient roomClient) =>
{
  var result = await roomClient.CoolAsync(new Temperature { Value = temperature });
  return Results.Ok(result.Value); // Assuming Value is a bool field in the response
});


app.Run();