using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reddit.AuthTokenRetriever;

namespace GRRAbstimmung.Helper
{

	internal class TokenStuff
	{
		// Stolen from their github https://github.com/sirkris/Reddit.NET/blob/master/docs/examples/cs/Authorize%20New%20User.md
		// 
		// just need to have redirect URL to be set correctly
		// http://localhost:8765/Reddit.NET/oauthRedirect


		string AuthorizeUser(string appId, string appSecret, int port = 8765)
		{
			// Create a new instance of the auth token retrieval library.  --Kris
			AuthTokenRetrieverLib authTokenRetrieverLib = new AuthTokenRetrieverLib(appId, appSecret, port);

			// Start the callback listener.  --Kris
			// Note - Ignore the logging exception message if you see it.  You can use Console.Clear() after this call to get rid of it if you're running a console app.
			authTokenRetrieverLib.AwaitCallback();

			// Open the browser to the Reddit authentication page.  Once the user clicks "accept", Reddit will redirect the browser to localhost:8080, where AwaitCallback will take over.  --Kris
			OpenBrowser(authTokenRetrieverLib.AuthURL());

			// Replace this with whatever you want the app to do while it waits for the user to load the auth page and click Accept.  --Kris
			while (true) { }

			// Cleanup.  --Kris
			authTokenRetrieverLib.StopListening();

			return authTokenRetrieverLib.RefreshToken;
		}

		void OpenBrowser(string authUrl, string browserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe")
		{
			try
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
				Process.Start(processStartInfo);
			}
			catch (System.ComponentModel.Win32Exception)
			{
				// This typically occurs if the runtime doesn't know where your browser is.  Use BrowserPath for when this happens.  --Kris
				ProcessStartInfo processStartInfo = new ProcessStartInfo(browserPath)
				{
					Arguments = authUrl
				};
				Process.Start(processStartInfo);
			}
		}

	}
}
