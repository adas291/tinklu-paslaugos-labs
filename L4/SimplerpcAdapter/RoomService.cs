namespace Servers;

using Services;


using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Grpc.Net.Client;

/// <summary>
/// Room conditioning service
/// </summary>
public class RoomService : IRoomService
{
  //NOTE: instance-per-request service would need logic to be static or injected from a singleton instance
  public Room.RoomClient roomClient { get; set; }
  public RoomService()
  {

    var channel = GrpcChannel.ForAddress("http://localhost:5000");
    roomClient = new Room.RoomClient(channel);
    // var grpcService = new Room.RoomClient(channel);

  }

  /// <summary>
  /// Thread safe access to current room temperature
  /// </summary>
  /// <returns>Current room temperature</returns>
  public float GetTemperature()
  {
    // return mLogic.GetRoomTemperature();
    return 0;
  }

  /// <summary>
  /// Access to minimal room temperature constant
  /// </summary>
  /// <returns>lowest room temperature limit</returns>
  public int GetMinTemperature()
  {
    return 0;
  }

  /// <summary>
  /// Access to maximum room temperature constant
  /// </summary>
  /// <returns>maximum room temperature limit</returns>
  public int GetMaxTemperature()
  {
    return 0;
  }

  /// <summary>
  /// Thread safe room temperature increase method
  /// </summary>
  /// <param name="degrees">temperature change value</param>
  /// <returns>true if room temperature changed</returns>
  public bool HeatRoom(float degrees)
  {
    // return mLogic.HeatRoom(degrees);
    var result = roomClient.Heat(new Temperature { Value = degrees });
    return result.Value;
  }

  /// <summary>
  /// Thread safe room temperature decrease method
  /// </summary>
  /// <param name="degrees">temperature change value</param>
  /// <returns>true if room temperature changed</returns>
  public bool CoolRoom(float degrees)
  {
    var result = roomClient.Cool(new Temperature { Value = degrees });
    return result.Value;
  }

  /// <summary>
  /// Check if room temperature is lower than min limit
  /// </summary>
  /// <returns>true if temperature > high limit</returns>
  public bool CanHeat()
  {
    // return mLogic.CanHeat();
    var result = roomClient.CanHeat(new Empty { });
    return result.Value;
  }

  /// <summary>
  /// Check if room temperature is higher than upper limit
  /// </summary>
  /// <returns>true if temperature > high limit </returns>
  public bool CanCool()
  {
    var result = roomClient.CanCool(new Empty { });
    return result.Value;
  }

}


/* public class RoomService() : IRoomService
{
  public bool CanCool()
  {
    throw new NotImplementedException();
  }

  public bool CanHeat()
  {
    throw new NotImplementedException();
  }

  public bool CoolRoom(float change)
  {
    throw new NotImplementedException();
  }

  public int GetMaxTemperature()
  {
    throw new NotImplementedException();
  }

  public int GetMinTemperature()
  {
    throw new NotImplementedException();
  }

  public float GetTemperature()
  {
    throw new NotImplementedException();
  }

  public bool HeatRoom(float change)
  {
    throw new NotImplementedException();
  }
} */