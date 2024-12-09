namespace Clients;

using System.Net.Http;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using NLog;

using Services;


/// <summary>
/// Client for heater
/// </summary>
class Client
{

  /// <summary>
  /// Heater client logger instance
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
  /// heater client body
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
        //connect to the server, get service proxy
        var channel = GrpcChannel.ForAddress("http://127.0.0.1:5000");
        var room = new Room.RoomClient(channel);

        while (true)
        {

          Thread.Sleep(rnd.Next(3000));

          // var tmpChange = (TemperatureMsg)(rnd.NextDouble() * 2);
          var tmpChange = new TemperatureMsg();
          tmpChange.Value = (float)rnd.NextDouble() * 2;

          if (room.CanHeat(new Empty()).Value && room.Heat(tmpChange).Value)
          {
            mLog.Info($"Heater increased room temperature by {tmpChange:F2}");
          }
          else
          {
            mLog.Info($"Heater is off");
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