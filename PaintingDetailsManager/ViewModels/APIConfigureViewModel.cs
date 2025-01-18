using Caliburn.Micro;
using PaintingDetailsManager.Utilities;
using PaintingLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PaintingDetailsManager.ViewModels
{
    public class APIConfigureViewModel : Screen
    {
        // public string MyProperty => ExternalProgram.GetCurrentValue();
        public APIConfigureViewModel()
        {
            _ipAddress = APIHelper.LoadIPAddress();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => NotifyOfPropertyChange(() => ProcessName);
            _timer.Start();
        }
        private Process webAPIProcess;

        private string _ipAddress = "7150";

		public string IPAddress
		{
			get { return _ipAddress; }
		}

		private string _portNumber;

		public string PortNumber
		{
			get { return _portNumber; }
			set 
			{ 
				_portNumber = value; 
				NotifyOfPropertyChange(() => PortNumber);
				NotifyOfPropertyChange(() => APIURL);
			}
		}

        public string APIURL { get { return $"{PortNumber}:{IPAddress}"; } }

        public void StartAPI()
        {
            try
            {
                var dllPath = Path.GetFullPath(@"..\..\..\SynchronizationAPI\bin\Debug\net8.0\SynchronizationAPI.dll");

                webAPIProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = dllPath,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                // Attach output and error handlers
                webAPIProcess.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
                webAPIProcess.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

                // Start the process and begin reading output
                webAPIProcess.Start();
                webAPIProcess.BeginOutputReadLine();
                webAPIProcess.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting API: {ex.Message}");
                if (webAPIProcess != null && !webAPIProcess.HasExited)
                {
                    webAPIProcess.Kill();
                    webAPIProcess.Dispose();
                }
                webAPIProcess = null;
            }
        }

        public void CheckProcessStatus()
        {
            if (webAPIProcess == null || webAPIProcess.HasExited)
            {
                ProcessName = webAPIProcess == null
                    ? "Web API is not running right now."
                    : $"Web API exited with code {webAPIProcess.ExitCode}.";
            }
            else
            {
                ProcessName = webAPIProcess.ProcessName;
            }
        }

        public void StopAPI()
        {
            try
            {
                if (webAPIProcess != null && !webAPIProcess.HasExited)
                {
                    webAPIProcess.Kill();
                    webAPIProcess.Dispose();
                    webAPIProcess = null;
                }
                ProcessName = "Web API is not running right now.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping API: {ex.Message}");
            }
        }


        private string _processName;
		public string ProcessName
		{
			get { return _processName; }
			set { _processName = value; NotifyOfPropertyChange(() => ProcessName); }
		}


		/*public void StopAPI()
		{
			try
			{
				// If it is null, we may not have even started it.
				if (webAPIProcess != null)
				{
					if (!webAPIProcess.HasExited) // If it hasn't exited, let's kill the process.
					{
						webAPIProcess.Kill();
					}
					
					// Now dispose of the object, set back to null, reset process name value.
					webAPIProcess.Dispose();
					webAPIProcess = null;
					ProcessName = "Web API is not running right now.";
				}
				ProcessName = "Web API is not running right now.";
            }
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}*/

        private readonly DispatcherTimer _timer;
    }
}
