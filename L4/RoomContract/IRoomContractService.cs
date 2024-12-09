namespace Services;


/// <summary>
/// Service contract.
/// </summary>
public interface IRoomService
{

  /// <summary>
  /// gets minimum value for a room
  /// </summary>
  /// <returns>temperature value</returns>
  int GetMinTemperature();

  /// <summary>
  /// gets maximum value for a room
  /// </summary>
  /// <returns>temperature value</returns>
  int GetMaxTemperature();

  /// <summary>
  /// Gets current room temperature
  /// </summary>
  /// <returns>temperature value</returns>
  float GetTemperature();

  /// <summary>
  /// Heats room by change value
  /// </summary>
  /// <returns>True if room was temperature was increased</returns>
  bool HeatRoom(float change);

  /// <summary>
  /// Cools down room by change value
  /// </summary>
  /// <returns>True if room was cooled down</returns>
  bool CoolRoom(float change);

  /// <summary>
  /// Checks room temperature can be heated
  /// </summary>
  /// <returns>true if room temeprature can be increased</returns>
  bool CanHeat();

  /// <summary>
  /// Checks room temperature can be decreased
  /// </summary>
  /// <returns>true if room temeprature can be decreased</returns>
  bool CanCool();
}
