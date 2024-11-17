using Caliburn.Micro;
using PaintingDetailsManager.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager.ViewModels
{
    public class APIConfigureViewModel : Screen
    {
		private string _ipAddress = "7150";

		public string IPAddress
		{
			get { return _ipAddress; }
		}

		private string _portNumber;

		public string PortNumber
		{
			get { return _portNumber; }
			set { 
					_portNumber = value; 
					NotifyOfPropertyChange(() => PortNumber);
				NotifyOfPropertyChange(() => APIURL);
			}
		}

        public string APIURL { get { return $"{PortNumber}:{IPAddress}"; } }

        public APIConfigureViewModel()
        {
			_ipAddress = APIHelper.LoadIPAddress();
        }

		public void StartAPI()
		{
			// PortNumber = "10";
		}

		public void StopAPI()
		{
			// PortNumber = "15";
		}


    }
}
