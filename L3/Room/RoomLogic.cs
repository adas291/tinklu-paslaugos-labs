namespace Servers;

using System.ComponentModel;
using NLog;
using NLog.MessageTemplates;
using Services;

/// <summary>
/// Traffic light state descritor.
/// </summary>
public class RoomTemperatureState
{
  public readonly object AccessLock = new object();

  public float Temperature { get; set; }

}


/// <summary>
/// <para>Traffic light logic.</para>
/// <para>Thread safe.</para>
/// </summary>
class RoomLogic
{
  /// <summary>
  /// Logger for this class.
  /// </summary>
  private Logger mLog = LogManager.GetCurrentClassLogger();

  /// <summary>
  /// Background task thread.
  /// </summary>
  private Thread mBgTaskThread;
  private RoomTemperatureState mState = new RoomTemperatureState();

  /// <summary>
  /// State descriptor.
  /// </summary>
  // private RoomTemperatureState mState = new RoomTemperatureState(18, 27);
  public readonly int LowTemperature;
  public readonly int HighTemperature;
  public readonly int Offset;



  /// <summary>
  /// Constructor.
  /// </summary>
  public RoomLogic(int initialTemperature = 22, int lowTemperature = 20, int highTemperature = 30, int offset = 5)
  {
    //start the background task
    LowTemperature = lowTemperature;
    HighTemperature = highTemperature;
    Offset = offset;
    mState.Temperature = initialTemperature;
    mBgTaskThread = new Thread(BackgroundTask);
    mBgTaskThread.Start();

  }


  /// <summary>
  /// Get current light state.
  /// </summary>
  /// <returns>Current light state.</returns>				
  public float GetRoomTemperature()
  {
    lock (mState.AccessLock)
    {
      return mState.Temperature;
    }
  }

  /// <summary>
  /// get max temperature limit constant for room. 
  /// </summary>
  /// <returns></returns>
  public int GetMinTemperature()
  {
    return LowTemperature;
  }

  /// <summary>
  /// get max temperature limit constant for room. 
  /// </summary>
  /// <returns></returns>
  public int GetMaxTemperature()
  {
    return HighTemperature;
  }


  /// <summary>
  /// Threadsafe heating of room
  /// </summary>
  /// <param name="temperature">temperature increase value</param>
  /// <returns>true if room was heated, false otherwise</returns>
  public bool HeatRoom(float temperature)
  {
    lock (mState.AccessLock)
    {
      if (mState.Temperature < LowTemperature)
      {
        temperature = Math.Min(LowTemperature + Offset, mState.Temperature + temperature);
        mState.Temperature = temperature;
        return true;
      }

      return false;
    }
  }


  /// <summary>
  /// Threadsafe cooling of room
  /// </summary>
  /// <param name="temperature">temperature decrease value</param>
  /// <returns>true if room was cooled down, false otherwise</returns>
  public bool CoolRoom(float temperature)
  {
    lock (mState.AccessLock)
    {
      if (mState.Temperature > HighTemperature)
      {
        temperature = Math.Max(HighTemperature - Offset, mState.Temperature - temperature);
        mState.Temperature = temperature;
        return true;
      }
      return false;
    }
  }

  /// <summary>
  /// Background task for the room conditioning
  /// </summary>
  public void BackgroundTask()
  {
    //intialize random number generator
    var rnd = new Random();

    mLog.Info($"Starting room temperature measurement...");
    mLog.Info($"Initial room temperature: '{mState.Temperature}°C");
    Console.Title = "Room monitor";

    while (true)
    {

      Thread.Sleep(2000);

      lock (mState.AccessLock)
      {
        var change = rnd.Next(0, 2) + rnd.NextDouble();

        var sign = rnd.Next(0, 2) == 0;

        if (sign)
        {
          change = -change;
        }

        mState.Temperature += (float)change;

        var signStr = change > 0 ? "+" : "";

        mLog.Info($"New room temperature is {mState.Temperature:F2}°C (change: {signStr}{change:F2}°C)'.");
      }
    }
  }

  public bool CanHeat()
  {
    return GetRoomTemperature() < GetMinTemperature();
  }

  public bool CanCool()
  {
    return GetRoomTemperature() > GetMaxTemperature();
  }

}