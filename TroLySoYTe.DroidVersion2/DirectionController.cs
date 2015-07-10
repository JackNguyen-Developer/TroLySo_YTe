using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;
using Android.Gms.Location;
using Android.Locations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Compression;

namespace TroLySoYTe.DroidVersion2
{
			
	public class DirectionController 
	{
		Context context;

		Uri url;
		double myLatitude, myLongitude, desLatitude, desLongitude;
		string keyAPI = null;
		string savePath = null;
		string fileName = null;
		string stringJSON = null;
		string point = null;
		public List<myLocation> listLocation;

		public DirectionController(Context context, double myLongitude, double myLatitude, double desLatitude, double desLongitude, string keyAPI)
		{
			this.context = context;
			savePath = "data/data/" + context.PackageName + "/";
			fileName = "direction.json";
			this.myLatitude = myLatitude; this.desLatitude = desLatitude;
			this.myLongitude = myLongitude; this.desLongitude = desLongitude;
			this.keyAPI = keyAPI;
			bool checkDown = downloadJSON ();
			if (checkDown) {
				point = readJSON ();
				if (point != null)
					listLocation = DecodePolylinePoints (point);
				else 
					Toast.MakeText (context, "not read string point in file JSON!", ToastLength.Short);
			} else {
				Toast.MakeText (context, "not download file JSON!", ToastLength.Short);
			}
		}
		//handle download file JSON, if downloaded return true;
		WebClient webClient;
		private bool downloadJSON()
		{
			try{
				webClient = new WebClient ();
				//webClient.Encoding = Encoding.UTF8;
				url = new Uri("http://maps.googleapis.com/maps/api/directions/json?origin=" + myLongitude + "," +  myLatitude + "&destination=" + desLatitude + "," + desLongitude +"&sensor=true&region=gb");
				using(Stream myStream = webClient.OpenRead(url)){
					StreamReader myStreamReader = new StreamReader(myStream, Encoding.UTF8);
					//string m = new StreamReader(myStream, Encoding.UTF8).ReadToEnd;
					var textJSON = myStreamReader.ReadToEnd();
					stringJSON = textJSON.ToString();
				}
				//webClient.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) => 
				//{
				//	var text = e.Result;
				//	File.WriteAllText(savePath + fileName, text);
				//};
				//webClient.Encoding = Encoding.UTF8;
				//url = new Uri("http://maps.googleapis.com/maps/api/directions/json?origin=" + myLongitude + "," +  myLatitude + "&destination=" + desLatitude + "," + desLongitude +"&sensor=true&region=gb");
				//webClient.DownloadStringAsync (url);
				return true;
			}catch(Exception e) {
				return false;
			}
		}

		//read file JSON and return string point
		private string readJSON()
		{
			//string stringJSON = File.ReadAllText (savePath + fileName);
			var jObject = JObject.Parse (stringJSON);
			var stringRoutes = jObject ["routes"] [0] ["overview_polyline"]["points"];
			return stringRoutes.ToString();
		}
		//decode string point
		private List<myLocation> DecodePolylinePoints(string encodedPoints) 
		{
			if (encodedPoints == null || encodedPoints == "") return null;
			List<myLocation> poly = new List<myLocation>();
			char[] polylinechars = encodedPoints.ToCharArray();
			int index = 0;
			int currentLat = 0;
			int currentLng = 0;
			int next5bits;
			int sum;
			int shifter;

			try
			{
				while (index < polylinechars.Length)
				{
					// calculate next latitude
					sum = 0;
					shifter = 0;
					do
					{
						next5bits = (int)polylinechars[index++] - 63;
						sum |= (next5bits & 31) << shifter;
						shifter += 5;
					} while (next5bits >= 32 && index < polylinechars.Length);

					if (index >= polylinechars.Length)
						break;

					currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

					//calculate next longitude
					sum = 0;
					shifter = 0;
					do
					{
						next5bits = (int)polylinechars[index++] - 63;
						sum |= (next5bits & 31) << shifter;
						shifter += 5;
					} while (next5bits >= 32 && index < polylinechars.Length);

					if (index >= polylinechars.Length && next5bits >= 32)
						break;

					currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
					myLocation p = new myLocation();
					p.latitude = Convert.ToDouble(currentLat) / 100000.0;
					p.longitude = Convert.ToDouble(currentLng) / 100000.0;
					poly.Add(p);
				} 
			}
			catch (Exception ex)
			{
				Toast.MakeText (context, "not decode string point location!", ToastLength.Short);
			}
			return poly;
		}
	
	
	}
}

