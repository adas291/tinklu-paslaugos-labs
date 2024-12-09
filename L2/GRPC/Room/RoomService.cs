namespace Servers;
using Grpc.Core;
using Services;


/// <summary>
/// Service
/// </summary>
public class RoomService : Services.Room.RoomBase
{
  //NOTE: instance-per-request service would need logic to be static or injected from a singleton instance
  private readonly RoomLogic mLogic = new RoomLogic();

  public override Task<Temperature> GetCurrentTemperature(Empty input, ServerCallContext context)
  {
    var result = new Temperature { Value = mLogic.GetRoomTemperature() };
    // var result = mLogic.GetRoomTemperature()
    return Task.FromResult(result);
  }

  public override Task<BoolMsg> CanCool(Empty input, ServerCallContext context)
  {
    var result = new BoolMsg { Value = mLogic.CanCool() };
    return Task.FromResult(result);
  }

  public override Task<BoolMsg> CanHeat(Empty input, ServerCallContext context)
  {
    var result = new BoolMsg { Value = mLogic.CanHeat() };
    return Task.FromResult(result);
  }

  public override Task<BoolMsg> Heat(Temperature input, ServerCallContext context)
  {
    var result = new BoolMsg { Value = mLogic.HeatRoom(input.Value) };
    return Task.FromResult(result);
  }

  public override Task<BoolMsg> Cool(Temperature input, ServerCallContext context)
  {
    var result = new BoolMsg { Value = mLogic.CoolRoom(input.Value) };
    return Task.FromResult(result);
  }

}