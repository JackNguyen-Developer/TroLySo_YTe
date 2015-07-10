using System;
using Android.App;
using Android.Runtime;

namespace TroLySoYTe.DroidVersion2
{
	//[Application]
	[Application (Label = "@string/app_name", Icon = "@drawable/icon_main", Theme = "@style/MyTheme")]
	public class AppController:Application
	{
		public AppController (IntPtr javaReference, JniHandleOwnership tranfer) : base (javaReference, tranfer)
		{	
		}

		public override void OnCreate ()
		{
			base.OnCreate ();
			ServiceContainer.Register<IConnection> (() => new DataAccessLayer (this));
			ServiceContainer.Register<MainViewModel> (() => new MainViewModel ());
		}

		public static IConnection getDAl ()
		{
			return ServiceContainer.Resolve<IConnection> ();
		}
	}
}

