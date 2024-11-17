using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager.Utilities
{
    /// <summary>
    /// This class contains many different helper functions which assist
    /// in API setup.
    /// </summary>
    public static class APIHelper
    {
        /// <summary>
        /// Gets the local IP address of this machine.
        /// </summary>
        /// <returns>Local IP Address as a string.</returns>
        public static string LoadIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }
    }
}
