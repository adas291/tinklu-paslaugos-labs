namespace Servers;

using Services;

/// <summary>
/// Room conditioning service
/// </summary>
public class RoomService : IRoomService
{
  //NOTE: instance-per-request service would need logic to be static or injected from a singleton instance
  private readonly RoomLogic mLogic = new RoomLogic(initialTemperature: 30);


  /// <summary>
  /// Thread safe access to current room temperature
  /// </summary>
  /// <returns>Current room temperature</returns>
  public float GetTemperature()
  {
    return mLogic.GetRoomTemperature();
  }

  /// <summary>
  /// Access to minimal room temperature constant
  /// </summary>
  /// <returns>lowest room temperature limit</returns>
  public int GetMinTemperature()
  {
    return mLogic.GetMinTemperature();
  }

  /// <summary>
  /// Access to maximum room temperature constant
  /// </summary>
  /// <returns>maximum room temperature limit</returns>
  public int GetMaxTemperature()
  {
    return mLogic.GetMaxTemperature();
  }

  /// <summary>
  /// Thread safe room temperature increase method
  /// </summary>
  /// <param name="degrees">temperature change value</param>
  /// <returns>true if room temperature changed</returns>
  public bool HeatRoom(float degrees)
  {
    return mLogic.HeatRoom(degrees);
  }

  /// <summary>
  /// Thread safe room temperature decrease method
  /// </summary>
  /// <param name="degrees">temperature change value</param>
  /// <returns>true if room temperature changed</returns>
  public bool CoolRoom(float degrees)
  {
    return mLogic.CoolRoom(degrees);
  }

  /// <summary>
  /// Check if room temperature is lower than min limit
  /// </summary>
  /// <returns>true if temperature > high limit</returns>
  public bool CanHeat()
  {
    return mLogic.CanHeat();
  }

  /// <summary>
  /// Check if room temperature is higher than upper limit
  /// </summary>
  /// <returns>true if temperature > high limit </returns>
  public bool CanCool()
  {
    // return mLogic.GetRoomTemperature() > mLogic.GetMaxTemperature();
    return mLogic.CanCool();
  }

}