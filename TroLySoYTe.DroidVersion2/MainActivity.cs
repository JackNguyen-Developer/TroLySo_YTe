using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Views.InputMethods;
using System.Collections.Generic;
using System.Threading;

namespace TroLySoYTe.DroidVersion2
{
	//[Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon",Theme="@style/MyTheme")]
	//[Activity (MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan)]
	[Activity (MainLauncher = false, WindowSoftInputMode = SoftInput.AdjustPan, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : ActionBarActivity,Android.Text.ITextWatcher,AbsListView.IOnScrollListener
	{
		public MainViewModel viewModel;
		//private IConnection connection=AppController.getDAl();
		private Android.Support.V7.Widget.Toolbar mToolbar;
		private Android.Support.V7.Widget.Toolbar mToolbarBottom;
		private MyActionBarDrawerToggle mDrawerToggle;
		private DrawerLayout mDrawerLayout;
		private ListView mLeftDrawer;
		public ListView mListDrugStore;
		public DrugStoreListViewAdapter mDrugstoreAdapter;
		private EditText mSearchBox;
		public Android.Support.V4.App.FragmentManager mFragmentManager;
		private string[] arrMenuItem;
		private string[] arraytest = { "johnny", "Mushi", "Dendi", "XBoct", "Puppey" };
		ArrayAdapter<string> adt;
		Handler mHandler = new Handler ();
		Runnable mFilterTask;
		bool first = true;
		bool isLoading = false;
		public int limit = 10;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			//get view
			initializeViewModel ();			
			//fragment manger
			InitializerFragmentManager ();
			//
			CreateToolbar ();
			initializeDrugStoreList ();
			//

			CreateDrawerLayout ();
			customizeSearchView ();
			ZipHelper zip = new ZipHelper (this);
			zip.copyFromAssert ();
			zip.Unzip ();
			mFilterTask = new Runnable (fillItemList);
		}

		private void CreateDrawerLayout ()
		{
			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			mLeftDrawer = FindViewById<ListView> (Resource.Id.left_drawer);
			CreateLeftDrawer ();
			mDrawerToggle = new MyActionBarDrawerToggle (
				this,  												//host
				mDrawerLayout,										//layout
				Resource.String.openeDrawer,						//open message					
				Resource.String.closeDrawer);						//close message
			mDrawerLayout.SetDrawerListener (mDrawerToggle);
			SupportActionBar.SetHomeButtonEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);
			mDrawerToggle.SyncState ();
		}

		private void initializeDrugStoreList ()
		{			
			mListDrugStore = FindViewById<ListView> (Resource.Id.listview_drugstores);
			new LoadDefaultData (this).Execute ("abc");
			//viewModel.GetDrugStoreByKeyWord ("", 10);
			mListDrugStore.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {	
				DetailDialogFragment dialog = new DetailDialogFragment (viewModel.list_drugstore [e.Position], this);
				dialog.Show (mFragmentManager, "detail");
			};
		}

		public class LoadDefaultData : AsyncTask
		{
			MainActivity context;

			public LoadDefaultData (MainActivity c)
			{
				context = c;
			}

			protected override void OnPreExecute ()
			{
				base.OnPreExecute ();
			}

			protected override Java.Lang.Object DoInBackground (params Java.Lang.Object[] @params)
			{
				context.viewModel.GetDrugStoreByKeyWord ("", 10);
				return true;
			}

			protected override void OnPostExecute (Java.Lang.Object result)
			{
				context.mDrugstoreAdapter = new DrugStoreListViewAdapter (context, context.viewModel.list_drugstore);
				context.mListDrugStore.Adapter = context.mDrugstoreAdapter;
				context.mListDrugStore.SetOnScrollListener ((AbsListView.IOnScrollListener)context);
			}
		}

		public class LoadMoreData :AsyncTask
		{
			MainActivity context;
			int lastCount;
			bool hasMoreItem = true;

			public LoadMoreData (MainActivity c, int limit, int position)
			{
				context = c;
			}

			protected override void OnPreExecute ()
			{
				base.OnPreExecute ();
			}

			protected override Java.Lang.Object DoInBackground (params Java.Lang.Object[] @params)
			{
				context.limit += 10;
				lastCount = context.viewModel.list_drugstore.Count;
				context.viewModel.GetDrugStoreByKeyWord (context.mSearchBox.Text, context.limit);
				if (lastCount >= context.viewModel.list_drugstore.Count) {
					hasMoreItem = false;
					context.limit -= 10;
				}
				return true;
			}

			protected override void OnPostExecute (Java.Lang.Object result)
			{					
				if (hasMoreItem == true) {
					int position = context.mListDrugStore.LastVisiblePosition;			
					context.mDrugstoreAdapter = new DrugStoreListViewAdapter (context, context.viewModel.list_drugstore);
					context.mListDrugStore.Adapter = context.mDrugstoreAdapter;
					context.mListDrugStore.SetSelectionFromTop (position - 1, 0);
					hasMoreItem = false;
				}		
			}
		}

		public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
		{						
		}

		int countRef = 0;

		public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
		{
			int threshold = 1;
			int count = mListDrugStore.Count;
			int position = mListDrugStore.LastVisiblePosition;
			if (scrollState == ScrollState.Idle) {
				if (position >= count - threshold) {
					new LoadMoreData (this, limit, position).Execute ("abc");
				}
				//Toast.MakeText (this, position.ToString (), ToastLength.Short).Show ();
			}
		}

		private void CreateToolbar ()
		{
			mToolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
			SetSupportActionBar (mToolbar);
		}

		private void CreateLeftDrawer ()
		{
			arrMenuItem = Resources.GetStringArray (Resource.Array.slidingMenuItem);
			LeftMenuItem adapter = new LeftMenuItem (this, arrMenuItem);
			mLeftDrawer.Adapter = adapter;
		}

		private void initializeViewModel ()
		{
			viewModel = ServiceContainer.Resolve<MainViewModel> ();
			viewModel.DAL = AppController.getDAl ();
		}

		private void InitializerFragmentManager ()
		{
			mFragmentManager = SupportFragmentManager;
		}

		public void customizeSearchView ()
		{			
			mSearchBox = FindViewById<EditText> (Resource.Id.search_box);
			mSearchBox.AddTextChangedListener (this);
			mSearchBox.FocusableInTouchMode = true;
			//mSearchBox.SetBackgroundResource (Resource.Drawable.test);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			mDrawerToggle.OnOptionsItemSelected (item);
			if (item.ItemId == Resource.Id.action_search) {				
				RelativeLayout layout = FindViewById<RelativeLayout> (Resource.Id.layout_search);

				if (layout.Visibility == ViewStates.Gone) {
					item.SetIcon (Resource.Drawable.ic_close);
					layout.Visibility = ViewStates.Visible;
					layout.Alpha = 0.0f;
					layout.Animate ().TranslationY (0).Alpha (1.0f);
				} else {
					item.SetIcon (Resource.Drawable.ic_search_white);
					layout.Visibility = ViewStates.Gone;
					InputMethodManager imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
					imm.HideSoftInputFromWindow (mSearchBox.WindowToken, 0);
				}
			}
			return base.OnOptionsItemSelected (item);
		}

		bool exit = false;

		public override bool OnKeyDown (Keycode keyCode, KeyEvent e)
		{
			if (keyCode == Keycode.Back) {
				if (exit == false) {
					Toast.MakeText (this, "Nhan lan nua de thoat", ToastLength.Long).Show ();
					exit = true;
				} else
					Finish ();
				return true;
			}
			return base.OnKeyDown (keyCode, e);
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{			
			MenuInflater.Inflate (Resource.Menu.action_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		class Runnable : Java.Lang.Object, Java.Lang.IRunnable
		{
			Action action;

			public Runnable (Action action)
			{
				this.action = action;
			}

			public void Run ()
			{
				action ();
			}
		}




		public void run ()
		{

		}

		public void fillItemList ()
		{
			if (mSearchBox.Text != "") {
				viewModel.GetDrugStoreByKeyWord (mSearchBox.Text);
			} else {
				if (first == false)
					viewModel.list_drugstore.Clear ();
			}
			mDrugstoreAdapter = new DrugStoreListViewAdapter (this, viewModel.list_drugstore);
			mListDrugStore.Adapter = mDrugstoreAdapter;		
		}

		public void AfterTextChanged (Android.Text.IEditable s)
		{		
			limit = 10;				
			if (first)
				first = false;
			mHandler.RemoveCallbacks (mFilterTask);
			mHandler.PostDelayed (mFilterTask, 850);
		}

		public void BeforeTextChanged (Java.Lang.ICharSequence s, int start, int count, int after)
		{				
		}

		public void OnTextChanged (Java.Lang.ICharSequence s, int start, int before, int count)
		{	
				
		}
	}

}


