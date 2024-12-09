namespace Servers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Room controller for controlling temperature   
/// </summary>
[Route("/room")]
[ApiController]
public class RoomController : ControllerBase
{
  /// <summary>
  /// Service logic. This is created in Server.StartServer() and received through DI in constructor.
  /// </summary>
  private readonly RoomLogic mLogic;

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="logic">Logic to use. This will get passed through DI.</param>
  public RoomController(RoomLogic logic)
  {
    this.mLogic = logic;
  }

  // /// <summary>
  // /// Get current room temperature
  // /// </summary>
  // /// <returns>Current light state.</returns>				
  // [HttpGet("/getRoomTemperature")]
  // public ActionResult<float> GetTemperature()
  // {
  //   return mLogic.GetRoomTemperature();
  // }

  /// <summary>
  /// Cool room by specified delta  
  /// </summary>
  /// <param name="temperature">Temperature change</param>
  /// <returns>True on success, false on failure.</returns>
  [HttpPost("/coolRoom")]
  public ActionResult<bool> CoolRoom(float temperature)
  {
    return mLogic.CoolRoom(temperature);
  }

  /// <summary>
  /// Heat room by specified delta  
  /// </summary>
  /// <param name="temperature">Temperature change</param>
  /// <returns>True on success, false on failure.</returns>
  [HttpPost("/heatRoom")]
  public ActionResult<bool> HeatRoom(float temperature)
  {
    return mLogic.HeatRoom(temperature);
  }

  /// <summary>
  /// Check if room can be heated 
  /// </summary>
  /// <returns>True if, can be heated, false othervise</returns>
  [HttpPost("/canHeat")]
  public ActionResult<bool> CanHeatRoom()
  {
    return mLogic.CanHeat();
  }

  /// <summary>
  /// Check if room needs to be cooled
  /// </summary> 
  /// <returns>True if, can be cooled, false othervise</returns>
  [HttpPost("/canCool")]
  public ActionResult<bool> CanCoolRoom()
  {
    return mLogic.CanCool();
  }
}