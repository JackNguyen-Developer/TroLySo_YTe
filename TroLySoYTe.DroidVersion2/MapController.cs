using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Location;
using Android.Locations;
using Android.Gms.Common;
using Android.Graphics;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Collections;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TroLySoYTe.DroidVersion2
{
	[ Activity (MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]	
	public class MapController : ActionBarActivity, IGooglePlayServicesClientConnectionCallbacks, IGooglePlayServicesClientOnConnectionFailedListener, 
	Android.Gms.Location.ILocationListener, View.IOnTouchListener, Android.Text.ITextWatcher, ListView.IOnItemClickListener
	{
		private GoogleMap googleMap;
		private MapFragment mapFragment;
		private LocationClient locationClient;
		private double myLatitude, myLongitude;
		private double hcmLatitude = 10.7743017;
		private double hcmLongitude = 106.6921853;
		private double drugLatitude = 0, drugLongitude = 0;

		//private static bool isLocation = false;

		private DrugStore store;
		private MainViewModel viewModel;

		public MapController ()
		{
			store = new DrugStore ();
			viewModel = ServiceContainer.Resolve<MainViewModel> ();
			viewModel.DAL = AppController.getDAl ();
		}



		protected override void OnResume ()
		{
			base.OnResume ();
			var checkLocation = (LocationManager)GetSystemService (Context.LocationService);
			bool check = checkLocation.IsProviderEnabled (LocationManager.GpsProvider);
			if (check && (drugLatitude > 0) && (drugLongitude > 0) && (myLatitude > 0) && (myLongitude > 0)) {
				getMyLocation ();
				getDirection ();
			}
		}

		protected override void OnStart ()
		{
			base.OnStart ();
			locationClient.Connect ();
		}

		protected override void OnCreate (Bundle bundle)
		{		
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Map_Layout);
			//get data from activity main
			getDataIntent ();
			
			checkGoogleMap ();

			locationClient = new LocationClient (this, this, this);
			locationClient.Connect ();
			drugstoreMarker ();
			changeType ();
			btDirection ();

			layoutSlidingTop ();
		}

		RelativeLayout containerSliding;
		EditText searchBox;
		ListView listViewDrugStores;

		private void layoutSlidingTop ()
		{
			containerSliding = FindViewById<RelativeLayout> (Resource.Id.container);
			searchBox = FindViewById<EditText> (Resource.Id.search_box);
			listViewDrugStores = FindViewById<ListView> (Resource.Id.listview_drugstores);
			listViewDrugStores.OnItemClickListener = this;
		
			containerSliding.SetOnTouchListener (this);
			searchBox.AddTextChangedListener (this);

			handleSpinner ();

//			listViewDrugStores.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
//				googleMap.Clear ();
//				drugLatitude = viewModel.list_drugstore [e.Position].latitude;
//				drugLongitude = viewModel.list_drugstore [e.Position].longtitude;
//				drugstoreMarker ();
//			};
		}

		private void handleSpinner ()
		{
			
			var spinnerDistrict = FindViewById<Spinner> (Resource.Id.spinner_District);
			var spinnerWard = FindViewById<Spinner> (Resource.Id.spinner_Ward);
			var listAll = viewModel.DAL.GetAllDistrict ();
			ArrayList listNameDistrict = new ArrayList ();
			ArrayList listNameWard = new ArrayList ();
			var adapterDistrict = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerDropDownItem);
			for (int i = 0; i < listAll.Count; i++) {
				adapterDistrict.Add (listAll [i].name);
			}

			spinnerDistrict.Adapter = adapterDistrict;
			spinnerDistrict.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				//district
				StringBuilder districtName = null;
				if (e.Position != 0) {
					try {
						bool checkDistrict = viewModel.GetDrugStoreByDistrict (listAll [e.Position].name.Trim (), null);
						if (checkDistrict) {
							mDrugstoreAdapter = new DrugStoreListViewAdapter (this, viewModel.list_drugstore);
							listViewDrugStores.Adapter = mDrugstoreAdapter;
						} else
							listViewDrugStores.Adapter = null;
					} catch (Exception ex) {
					}
				}
				//ward
				var adapterWard = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerDropDownItem);
				int positionDistrict = e.Position;
				for (int i = 0; i < listAll [positionDistrict].WardChild.Count; i++) {
					adapterWard.Add (listAll [positionDistrict].WardChild [i].name);
				}
				spinnerWard.Adapter = adapterWard;
				spinnerWard.ItemSelected += (object ow, AdapterView.ItemSelectedEventArgs ew) => {
					if (ew.Position != 0) {
						try {
							bool checkWard = viewModel.GetDrugStoreByDistrict (listAll [e.Position].name.Trim (), listAll [positionDistrict].WardChild [ew.Position].name.Trim ());
							if (checkWard) {
								mDrugstoreAdapter = new DrugStoreListViewAdapter (this, viewModel.list_drugstore);
								listViewDrugStores.Adapter = mDrugstoreAdapter;
							} else
								listViewDrugStores.Adapter = null;
						} catch (Exception ex) {
						}
					}
				};
			};
		}

		private void changeType ()
		{
			Button normal = FindViewById<Button> (Resource.Id.normal);
			normal.Click += (object sender, EventArgs e) => {
				googleMap.MapType = GoogleMap.MapTypeNormal;
			};
			Button satellite = FindViewById<Button> (Resource.Id.satellite);
			satellite.Click += (object sender, EventArgs e) => {
				googleMap.MapType = GoogleMap.MapTypeSatellite;
			};
			Button hybrid = FindViewById<Button> (Resource.Id.hybrid);
			hybrid.Click += (object sender, EventArgs e) => {
				googleMap.MapType = GoogleMap.MapTypeHybrid;
			};
		}

		Button direction;

		private void btDirection ()
		{
			direction = FindViewById<Button> (Resource.Id.direction);
			direction.Click += (object sender, EventArgs e) => {
				var checkLocation = (LocationManager)GetSystemService (Context.LocationService);
				bool check = checkLocation.IsProviderEnabled (LocationManager.GpsProvider);
				if (check) {
					getMyLocation ();
					getDirection ();
				} else {
					Intent intent = new Intent (Android.Provider.Settings.ActionLocationSourceSettings);
					StartActivity (intent);
				}

			};
		}

		private void drugstoreMarker ()
		{
			if (drugLatitude <= 0 || drugLongitude <= 0) {
				Toast.MakeText (this, "not latitude and longitude of Drugstore", ToastLength.Short);
				drugLatitude = hcmLatitude;
				drugLongitude = hcmLongitude;
			} else {
				LatLng lat = new LatLng (drugLatitude, drugLongitude);
				googleMap.AddMarker (createMarker (lat, store.name, store.addressDetail, false));
				moveCamera (lat, 15);
			}
		}

		private void getDataIntent ()
		{
			store.name = Intent.GetStringExtra ("name");
			store.addressDetail = Intent.GetStringExtra ("addressDetail");
			store.latitude = Intent.GetDoubleExtra ("latitude", 0);
			store.longtitude = Intent.GetDoubleExtra ("longtitude", 0); 
			store.profilePicutre = Intent.GetStringExtra ("profilePicutre");
			drugLatitude = store.latitude;
			drugLongitude = store.longtitude;
		}

		private void checkGoogleMap ()
		{
			mapFragment = (MapFragment)FragmentManager.FindFragmentById (Resource.Id.map);
			googleMap = mapFragment.Map;
		}

		LatLng latLng = null;
		//of IGooglePlayServicesClientConnectionCallbacks
		public void OnConnected (Bundle p0)
		{
			getMyLocation ();
		}

		Location location;

		private void getMyLocation ()
		{
			try {
				location = locationClient.LastLocation;
				myLatitude = location.Latitude;
				myLongitude = location.Longitude;
						
				if (myLatitude <= 0 && myLongitude <= 0) {
					latLng = new LatLng (hcmLatitude, hcmLongitude);
					googleMap.AddMarker (createMarker (latLng, "HCM city", "Not location!", false));
				} else {
					latLng = new LatLng (myLatitude, myLongitude);
					googleMap.AddMarker (createMarker (latLng, "It's me", "See phone!", true));
				}

			} catch (Exception e) {
				
			}

		}

		private void moveCamera (LatLng latLng, byte size)
		{
			if (size <= 0)
				size = 15;
			CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom (latLng, size);
			googleMap.MoveCamera (camera);
		}

		private List<myLocation> listLocation;

		private void getDirection ()
		{	
			DirectionController n = new DirectionController (this, myLatitude, myLongitude, drugLatitude, drugLongitude, "AIzaSyDq4KITG5eySbUdYn-FqHamOdrs6BKoVKU");
			listLocation = n.listLocation;
			LatLng mCamera = null;//move camera
			PolylineOptions polyOptions = new PolylineOptions ();
			foreach (myLocation item in listLocation) {
				LatLng addLat = new LatLng (item.latitude, item.longitude);
				if (item == listLocation [((listLocation.Count) / 2)]) {
					mCamera = new LatLng (item.latitude, item.longitude);
					moveCamera (mCamera, 0);
				}
				polyOptions.Add (addLat);
			}
			Polyline polyLine = googleMap.AddPolyline (polyOptions);
			polyLine.Color = Android.Graphics.Color.Blue;
			polyLine.Width = 5;

		}
		//create market: if my=true => load image mylocation
		private MarkerOptions createMarker (LatLng latLng, string title, string subTitle, bool my)
		{
			MarkerOptions option = new MarkerOptions ();
			option.SetPosition (latLng);
			option.SetTitle (title);
			option.SetSnippet (subTitle);
			if (my)
				option.InvokeIcon (BitmapDescriptorFactory.FromResource (Resource.Drawable.my_location));
			return option;
		}

		public void OnDisconnected ()
		{
			throw new NotImplementedException ();
		}

		//of IGooglePlayServicesClientOnConnectionFailedListener

		public void OnConnectionFailed (ConnectionResult result)
		{
			throw new NotImplementedException ();
		}
		//of Android.Gms.Location.ILocationListener
		public void OnLocationChanged (Android.Locations.Location location)
		{
			getMyLocation ();
		}
		//event touch
		private float layoutLastPosY;

		public bool OnTouch (View v, MotionEvent e)
		{
			
			switch (e.Action) {
			case MotionEventActions.Down:

				layoutLastPosY = e.GetY ();
				return true;

			case MotionEventActions.Move:
				
				var currentPosition = e.GetY ();
				var deltaY = layoutLastPosY - currentPosition;

				var transY = v.TranslationY;

				transY -= deltaY;

				if (transY < 0) {
					transY = 0;
				}

				v.TranslationY = transY;

				return true;
			
			default:
				return v.OnTouchEvent (e);
			}

		}
		//event edittext
		public void AfterTextChanged (Android.Text.IEditable s)
		{
			
		}

		public void BeforeTextChanged (Java.Lang.ICharSequence s, int start, int count, int after)
		{
			
		}

		private DrugStoreListViewAdapter mDrugstoreAdapter;
		List<DrugStore> list = new List<DrugStore> ();

		public void OnTextChanged (Java.Lang.ICharSequence s, int start, int before, int count)
		{
			//search by edittext
			viewModel.GetDrugStoreByKeyWord (s.ToString ());
			mDrugstoreAdapter = new DrugStoreListViewAdapter (this, viewModel.list_drugstore);
			listViewDrugStores.Adapter = mDrugstoreAdapter;
		}

		public void OnItemClick (AdapterView parent, View view, int position, long id)
		{
			googleMap.Clear ();
			drugLatitude = viewModel.list_drugstore [position].latitude;
			drugLongitude = viewModel.list_drugstore [position].longtitude;
			drugstoreMarker ();
		}
	}
}

