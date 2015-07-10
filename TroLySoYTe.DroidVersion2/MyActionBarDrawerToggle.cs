using System;
using SupportActioBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using Android.Support.V7.App;
using Android.Support.V4.Widget;


namespace TroLySoYTe.DroidVersion2
{
	public class MyActionBarDrawerToggle : SupportActioBarDrawerToggle
	{
		private ActionBarActivity mHostActivity;
		private int mOpendResource;
		private int mClosedResource;
		public MyActionBarDrawerToggle (ActionBarActivity host,DrawerLayout drawerLayout ,int openedResourse,int closedResource)
			:base(host,drawerLayout,openedResourse,closedResource)
		{
			mHostActivity = host;
			mOpendResource = openedResourse;
			mClosedResource = closedResource;
		}
		public override void OnDrawerOpened (Android.Views.View drawerView)
		{						
			base.OnDrawerOpened (drawerView);
		}
		public override void OnDrawerClosed (Android.Views.View drawerView)
		{
			base.OnDrawerClosed (drawerView);
		}
		public override void OnDrawerSlide (Android.Views.View drawerView, float slideOffset)
		{
			base.OnDrawerSlide (drawerView, slideOffset);
		}
	}
}

