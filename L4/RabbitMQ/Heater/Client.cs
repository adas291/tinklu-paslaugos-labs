namespace Clients;

using NLog;

using Services;


/// <summary>
/// Client example.
/// </summary>
class Client
{
  /// <summary>
  /// Logger for this class.
  /// </summary>
  Logger mLog = LogManager.GetCurrentClassLogger();

  /// <summary>
  /// Configures logging subsystem.
  /// </summary>
  private void ConfigureLogging()
  {
    var config = new NLog.Config.LoggingConfiguration();

    var console =
      new NLog.Targets.ConsoleTarget("console")
      {
        Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception}"
      };
    config.AddTarget(console);
    config.AddRuleForAllLevels(console);

    LogManager.Configuration = config;
  }

  /// <summary>
  /// Program body.
  /// </summary>
  private void Run()
  {
    //configure logging
    ConfigureLogging();

    //initialize random number generator
    var rnd = new Random();

    //run everythin in a loop to recover from connection errors
    while (true)
    {
      try
      {
        //connect to the server, get service client proxy
        var room = new RoomClient();

        while (true)
        {
          var delta = (float)rnd.NextDouble();

          Thread.Sleep(rnd.Next(1000, 1500));

          if (room.CanHeat() && room.HeatRoom(delta))
          {
            mLog.Info($"Heater increased temperature by: {delta}");
          } else {
            mLog.Info($"Heater is turned off");
          }
        }
      }
      catch (Exception e)
      {
        //log whatever exception to console
        mLog.Warn(e, "Unhandled exception caught. Will restart main loop.");

        //prevent console spamming
        Thread.Sleep(2000);
      }
    }
  }

  /// <summary>
  /// Program entry point.
  /// </summary>
  /// <param name="args">Command line arguments.</param>
  static void Main(string[] args)
  {
    var self = new Client();
    self.Run();
  }
}
