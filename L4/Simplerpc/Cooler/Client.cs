namespace Clients;

using Microsoft.Extensions.DependencyInjection;

using SimpleRpc.Serialization.Hyperion;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Client;

using NLog;

using Services;


/// <summary>
/// Cooler client.
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

    var rnd = new Random();
    mLog.Info($"Cooler monitor");
    Console.Title = $"Cooler monitor";

    //run everythin in a loop to recover from connection errors
    while (true)
    {
      try
      {
        //connect to the server, get service client proxy
        var sc = new ServiceCollection();
        sc.AddSimpleRpcClient(
            "trafficLightService", //must be same as on line 86
            new HttpClientTransportOptions
            {
              Url = "http://localhost:6001/simplerpc",
              Serializer = "HyperionMessageSerializer"
            }
          ).AddSimpleRpcHyperionSerializer();

        sc.AddSimpleRpcProxy<IRoomService>("trafficLightService"); //must be same as on line 77

        var sp = sc.BuildServiceProvider();

        var room = sp.GetService<IRoomService>();



        //log identity data
        // mLog.Info($"I am car {car.CarId}, RegNr. {car.CarNumber}, Driver {car.DriverNameSurname}.");
        // Console.Title = $"I am car {car.CarId}, RegNr. {car.CarNumber}, Driver {car.DriverNameSurname}.";

        //do the car stuff
        while (true)
        {
          Thread.Sleep(rnd.Next(4000));


          var tmpChange = (float)(rnd.NextDouble() * 2);

          if (room.CanCool() && room.CoolRoom(tmpChange))
          {
            mLog.Info($"Cooler decreased room temperature by -{tmpChange:F2} °C");
          }
          else
          {
            mLog.Info($"Cooler is off");
          }

        }
      }

      catch (Exception e)
      {
        //log whatever exception to console
        // mLog.Warn(e, "Unhandled exception caught. Will restart main loop.");

        mLog.Warn("Server is unreachable, trying again in 2 seconds");
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
