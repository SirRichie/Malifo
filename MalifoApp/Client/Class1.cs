using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Class1
    {
        public static void Main(String[] args)
        {
            // Verbindung zum Server aufbauen
            TcpClient c = new TcpClient("localhost", 4711);
            // Stream zum lesen holen
            StreamReader inStream = new StreamReader(c.GetStream());
            bool loop = true;
            while (loop)
            {
                try
                {
                    // Hole nächsten Zeitstring vom Server
                    String time = inStream.ReadLine();
                    // Setze das Schleifen-Flag zurück
                    // wenn der Server aufgehört hat zu senden
                    loop = !time.Equals("");
                    // Gib die Zeit auf der Console aus
                    Console.WriteLine(time);
                }
                catch (Exception)
                {
                    // Setze das Schleifen-Flag zurück
                    // wenn ein Fehler in der Kommunikation aufgetreten ist
                    loop = false;
                }
            }
            // Schließe die Verbindung zum Server
            c.Close();
        }
    }
}
