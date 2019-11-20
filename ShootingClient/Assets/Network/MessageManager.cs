using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Net;

namespace ShootingNetwork
{
	public static class MessageManager
	{
		static string LastResponse { set; get; }

		static CookieContainer cookies = new CookieContainer();
		public static HttpWebRequest GenerateRequest(string uri, string content, NetworkMethod method, string login, string password, bool allowAutoRedirect)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			// Create a request using a URL that can receive a post. 
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
			// Set the Method property of the request to POST.
			request.Method = "Get";
			// Set cookie container to maintain cookies
			request.CookieContainer = cookies;
			request.AllowAutoRedirect = allowAutoRedirect;
			// If login is empty use defaul credentials
			if (string.IsNullOrEmpty(login))
			{
				request.Credentials = CredentialCache.DefaultNetworkCredentials;
			}
			else
			{
				request.Credentials = new NetworkCredential(login, password);
			}
			if (method == NetworkMethod.Post)
			{
				// Convert POST data to a byte array.
				byte[] byteArray = Encoding.UTF8.GetBytes(content);
				// Set the ContentType property of the WebRequest.
				request.ContentType = "application/x-www-form-urlencoded";
				// Set the ContentLength property of the WebRequest.
				request.ContentLength = byteArray.Length;
				// Get the request stream.
				Stream dataStream = request.GetRequestStream();
				// Write the data to the request stream.
				dataStream.Write(byteArray, 0, byteArray.Length);
				// Close the Stream object.
				dataStream.Close();
			}
			return request;
		}

		public static HttpWebResponse GetResponse(HttpWebRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			HttpWebResponse response = null;
			try
			{
				response = (HttpWebResponse)request.GetResponse();
				cookies.Add(response.Cookies);
				// Print the properties of each cookie.
				Console.WriteLine("\nCookies: ");
				foreach (Cookie cook in cookies.GetCookies(request.RequestUri))
				{
					Console.WriteLine("Domain: {0}, String: {1}", cook.Domain, cook.ToString());
				}
			}
			catch (WebException ex)
			{
				Console.WriteLine("Web exception occurred. Status code: {0}", ex.Status);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return response;
		}
		public static string GetResponseContent(HttpWebResponse response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			Stream dataStream = null;
			StreamReader reader = null;
			string responseFromServer = null;
			try
			{
				// Get the stream containing content returned by the server.
				dataStream = response.GetResponseStream();
				// Open the stream using a StreamReader for easy access.
				reader = new StreamReader(dataStream);
				// Read the content.
				responseFromServer = reader.ReadToEnd();
				// Cleanup the streams and the response.
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
				if (dataStream != null)
				{
					dataStream.Close();
				}
				response.Close();
			}
			LastResponse = responseFromServer;
			return responseFromServer;
		}
	}

}