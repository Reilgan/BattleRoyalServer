using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace masterServer
{
    public class MasterServer : ApplicationBase
	{
		private readonly ILogger log = LogManager.GetCurrentClassLogger();
		protected override PeerBase CreatePeer(InitRequest initRequest)
		{
			return new MasterClient(initRequest);
		}

		protected override void Setup()
		{
			var file = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
			if (file.Exists)
			{
				LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
				XmlConfigurator.ConfigureAndWatch(file);
			}

			log.Debug("server start");
		}

		protected override void TearDown()
		{
			log.Debug("server stop");
		}
	}
}
