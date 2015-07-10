
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
using System.Threading;

namespace TroLySoYTe.DroidVersion2
{
	[Activity (MainLauncher = true, NoHistory = true, Theme = "@style/MyThemeLoad", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class LoadScreen : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Thread.Sleep (3000); // Simulate a long loading process on app startup.
			StartActivity (typeof(MainActivity));
			Finish ();
		}
	}
}

