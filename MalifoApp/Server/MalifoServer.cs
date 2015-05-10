using Server.configuration;
using Server.Services;
using Server.userManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
	public class MalifoServer : IDisposable
	{
		private ServerConfiguration _serverConfiguration;
		private static TcpListener _listener = null;
        private static IList<ServerThread> _threads = new List<ServerThread>();
        public bool _stopServer = false;
		
		public MalifoServer(ServerConfiguration serverConfiguration)
		{
			_serverConfiguration = serverConfiguration;                     
		}

        public void StartServer()
        {
            new Thread(new ThreadStart(RunServer)).Start();
        }

        public void StopServer()
        {
            _stopServer = true;
        }
        
		private void RunServer()
		{
            CheckConfiguration();

            _listener = new TcpListener(_serverConfiguration.LocalAddress, _serverConfiguration.Port.Value);
			_listener.Start();
		
			Thread th = new Thread(new ThreadStart(Run));
           
			th.Start();

            while (!_stopServer)
            {
                Thread.Sleep(2000);
			}
			
			th.Abort();

            StopClientThreads();
            _threads = null;		
			_listener.Stop();
            _listener = null;
		}

        private static void StopClientThreads()
        {
            for (IEnumerator e = _threads.GetEnumerator(); e.MoveNext(); )
            {
                ServerThread serverThread = (ServerThread)e.Current;
                serverThread.Stop = true;
                while (serverThread.Running)
                    Thread.Sleep(1000);
            }
        }

        private bool CheckConfiguration()
        {
            if (_serverConfiguration.LocalAddress == null)
            {
                throw new ArgumentException("LocalAddress couldn't be null");
            }
            if (_serverConfiguration.Port == null)
            {
                throw new ArgumentException("Port couldn't be null");
            }

            return true;
        }
		
		private void Run()		
		{
            Console.WriteLine();
			while (true) {				
				TcpClient clientThread = _listener.AcceptTcpClient();				
				_threads.Add(new ServerThread(clientThread));
                Thread.Sleep(100);
			}
		}

        public void Dispose()
        {
            if (_listener != null)
            {
                _listener.Stop();
            }
            if (_threads != null)
            {
                StopClientThreads();
            }
        }
    }
}
