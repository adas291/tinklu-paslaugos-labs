namespace Clients;

using Microsoft.Extensions.DependencyInjection;

using SimpleRpc.Serialization.Hyperion;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Client;

using NLog;

using Services;


/// <summary>
/// Heater client.
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
    ConfigureLogging();

    var rnd = new Random();
    mLog.Info($"Heater monitor");
    Console.Title = $"Heater monitor";

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
              Url = "http://127.0.0.1:5000/simplerpc",
              Serializer = "HyperionMessageSerializer"
            }
          ).AddSimpleRpcHyperionSerializer();

        sc.AddSimpleRpcProxy<IRoomService>("trafficLightService"); //must be same as on line 77

        var sp = sc.BuildServiceProvider();

        var room = sp.GetService<IRoomService>();

        //initialize car descriptor

        //log identity data
        // mLog.Info($"I am car {car.CarId}, RegNr. {car.CarNumber}, Driver {car.DriverNameSurname}.");
        // Console.Title = $"I am car {car.CarId}, RegNr. {car.CarNumber}, Driver {car.DriverNameSurname}.";


        //do the car stuff
        while (true)
        {

          Thread.Sleep(rnd.Next(3000));

          var tmpChange = (float)(rnd.NextDouble() * 2);

          if (room.CanHeat() && room.HeatRoom(tmpChange))
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
