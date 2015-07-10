using System;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using TroLySoYTe.DroidVersion2;
using Android.Content;
using Android.OS;
using Android.Net;
using Android.Locations;
using Android.App;

namespace TroLySoYTe.DroidVersion2
{
	public class DetailDialogFragment: Android.Support.V4.App.DialogFragment
	{
		Context context;
		DrugStore store;
		bool isWifi = false;

		public DetailDialogFragment (DrugStore item, Context context)
		{
			this.context = context;
			store = item;
		}

		View view;

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			view = inflater.Inflate (Resource.Layout.detail_dialogfragment, null);
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle);
			TextView name = view.FindViewById<TextView> (Resource.Id.detail_txt_item);
			name.Text = store.name;
			TextView address = view.FindViewById<TextView> (Resource.Id.detail_txt_item_address);
			address.Text = store.addressDetail;
			ImageView logo = view.FindViewById<ImageView> (Resource.Id.detail_img_item);
			logo.SetImageResource (Resource.Drawable.ic_store_default);
			ImageView closeIcon = view.FindViewById<ImageView> (Resource.Id.detail_icon_close);
			closeIcon.Click += (object sender, EventArgs e) => {
				Dialog.Dismiss ();
			};

			handleButton ();
			//

			return view;
		}

		private void handleButton ()
		{
			Button btn_viewmap = view.FindViewById<Button> (Resource.Id.btn_viewmap);
			btn_viewmap.Click += (object sender, EventArgs e) => {
				checkWifi ();
				if (isWifi) {
					var intent = new Intent (context, typeof(MapController));
					Bundle bundle = new Bundle ();
					bundle.PutString ("name", store.name);
					bundle.PutString ("addressDetail", store.addressDetail);
					bundle.PutDouble ("longtitude", store.longtitude);
					bundle.PutDouble ("latitude", store.latitude);
					bundle.PutString ("profilePicutre", store.profilePicutre);
					intent.PutExtras (bundle);
					context.StartActivity (intent);
					this.Dismiss ();
				}
			};
		}

		private void checkWifi ()
		{
			ConnectivityManager connManager = (ConnectivityManager)context.GetSystemService (Context.ConnectivityService);
			var activeConnect = connManager.ActiveNetworkInfo;
			if (activeConnect != null) {
				//have connect wifi
				isWifi = true;
			} else {
				questUser ();
			}
		}

		private void questUser ()
		{
			AlertDialog.Builder build = new AlertDialog.Builder (context);
			AlertDialog ad = build.Create ();
			ad.SetTitle ("Chưa có kết nối internet!");
			ad.SetMessage ("Bạn hãy kích hoạt internet: Wifi hoặc 3G");
			ad.SetButton ("Wifi", (s, e) => {
				Intent intentWifi = new Intent (Android.Provider.Settings.ActionWifiSettings);
				context.StartActivity (intentWifi);
			});
			ad.SetButton2 (" 3G ", (s, e) => {
				Intent intent3G = new Intent (Android.Provider.Settings.ActionDataRoamingSettings);
				context.StartActivity (intent3G);		
			});
			ad.Show ();
		}

	}
}

