﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
//using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Calendar
{
	public class PushNotificationManager
	{
		static string _uri = "https://fcm.googleapis.com/fcm/send";
		static string _serverkey = "AAAAgAP6i24:APA91bGqhhWmOBL82_hRjktq0Zlsi0-FdIgjmZ5h6E7NfoyT9IKQANwLuh8GFso265J1HQnDbXnygij_q5ClbFLmYvEg2ARUjEaL2Lmu8ZRWAQhNAMzs0ijsglwf9wDR69SmAZn7B0T8";
		static string _token = "fNxPStYKF3Q:APA91bE2xWYYW6TKQW7BELfZVLHWZ9naikitfS0myre84lqWtwgtmPjE2up1sY1BQB5YllJpcSL79phHYo8YeSvf59q7AxmrztDUF_C7_cR2fT-JeV4KlFQ0ve9sqwxKAwezH0hDn9sv";

		static public void SendPushNotification()
		{
			var tm = new TrashManager();
			var json = new JObject(
							new JProperty("to", _token),
							new JProperty("priority", "high"),
							new JProperty("notification", new JObject(
					new JProperty("title", "ゴミ出し"),
					new JProperty("body", tm.GetTodayTomorrowString() +tm.GetTrashString()+"です。")
							)));

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_uri);
			request.ContentType = "application/json";
			request.Headers[HttpRequestHeader.Authorization] = "key=" + _serverkey;
			request.Method = "POST";

			using (var streamWriter = new StreamWriter(request.GetRequestStreamAsync().Result))
			{
				streamWriter.Write(json);
				streamWriter.Flush();
				streamWriter.Dispose();
				try
				{
					var httpResponse = (HttpWebResponse)request.GetResponseAsync().Result;
					using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
					{
						var result = streamReader.ReadToEnd();
						Debug.WriteLine("Response Body: \r\n {0}", result);
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception {0}", ex);
				}
			}
		}
	}
}
