﻿using Server.configuration;
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

        public void StopServer()
        {
            _stopServer = true;
        }
        
		public void runServer()
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
			// Listener stoppen
			_listener.Stop();
            _listener = null;
		}

        private static void StopClientThreads()
        {
            for (IEnumerator e = _threads.GetEnumerator(); e.MoveNext(); )
            {

                ServerThread serverThread = (ServerThread)e.Current;
                serverThread.stop = true;
                while (serverThread.running)
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
			while (true) {
				// Wartet auf eingehenden Verbindungswunsch
				TcpClient clientThread = _listener.AcceptTcpClient();
				// Initialisiert und startet einen Server-Thread
				// und fügt ihn zur Liste der Server-Threads hinzu
				_threads.Add(new ServerThread(clientThread));
                Thread.Sleep(1);
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
