using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.configuration
{
	public class ServerConfiguration
	{
		public int? Port { get; set; }
		public int? MaxPlayer { get; set; }
        public IPAddress LocalAddress { get; set; }
    }
}
