using System;
using Android.Widget;
using Android.Content;
using System.Collections.Generic;
using Android.Views;
using Android.Graphics;
using System.IO;
using Android.OS;
using System.Threading.Tasks;
using Android.Content.Res;
using Java.Lang.Ref;

namespace TroLySoYTe.DroidVersion2
{
	public class DrugStoreListViewAdapter:BaseAdapter<DrugStore>
	{
		Context context;
		List<DrugStore> list;

		public override DrugStore this [int index] {
			get {
				return list [index];
			}
		}

		public DrugStoreListViewAdapter (Context context, List<DrugStore> list)
		{
			this.context = context;
			this.list = list;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		//		public class AsyncDrawable:Android.Graphics.Drawables.BitmapDrawable
		//		{
		//			Context context;
		//			Java.Lang.Ref.WeakReference reference;
		//
		//			public AsyncDrawable (BitmapWorkerTask  decoder, Bitmap bitmap) : base (bitmap)
		//			{
		//				reference = new Java.Lang.Ref.WeakReference (decoder);
		//			}
		//
		//			public BitmapWorkerTask  getDecoder ()
		//			{
		//				return (BitmapWorkerTask)reference.Get ();
		//			}
		//		}

		//		public class BitmapWorkerTask  : AsyncTask
		//		{
		//			public string path;
		//			Bitmap bm;
		//			string picturename;
		//			Context context;
		//			Java.Lang.Ref.WeakReference refer;
		//			AsyncDrawable drawable;
		//
		//			public BitmapWorkerTask (ImageView img, string picturename, Context c, string path)
		//			{
		//				context = c;
		//				refer = new Java.Lang.Ref.WeakReference (img);
		//				this.picturename = picturename;
		//				this.path = path;
		//			}
		//
		//			protected override void OnPreExecute ()
		//			{
		//				base.OnPreExecute ();
		//			}
		//
		//			protected override Java.Lang.Object DoInBackground (params Java.Lang.Object[] @params)
		//			{
		//
		//				bm = LoadingImageUtil.loadingBitmapFromPath (path, context);
		//				return true;
		//			}
		//
		//			protected override void OnPostExecute (Java.Lang.Object result)
		//			{
		//				drawable = new AsyncDrawable (this, bm);
		//				if (refer != null && bm != null) {
		//					ImageView im = (ImageView)refer.Get ();
		//					if (im != null)
		//						im.SetImageBitmap (drawable.Bitmap);
		//				}
		//			}
		//		}

		public bool cancelPotentialWork (string path, ImageView imageView)
		{
			BitmapWorkerTask decoder = getDecoder (imageView);
			if (decoder != null) {
				
				if (path != decoder.path || path == null) {
					decoder.Cancel (true);
				} else {
					return false;
				}
			}
			return true;
		}

		public BitmapWorkerTask  getDecoder (ImageView imageview)
		{
			if (imageview != null) {
				Android.Graphics.Drawables.Drawable drawable = imageview.Drawable;
				//Bitmap bm = ((Android.Graphics.Drawables.BitmapDrawable)imageview.Drawable).Bitmap;
				if (drawable is AsyncDrawable) {
					AsyncDrawable m = (AsyncDrawable)drawable;
					return m.getDecoder ();
				} 
			}
			return null;
		}

		public async void LoadScaledDownBitmapForDisplayAsync (Resources res, ImageView imv)
		{		
			BitmapFactory.Options option = new BitmapFactory.Options ();   
			Bitmap bitmapToDisplay = await BitmapFactory.DecodeResourceAsync (res, Resource.Drawable.ic_store_default, option);
			imv.SetImageBitmap (bitmapToDisplay);
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			LayoutInflater inflater = (LayoutInflater)context.GetSystemService (Context.LayoutInflaterService);
			//View view = inflater.Inflate (Resource.Layout.listview_item_drugstore, null, false);
			View view = convertView;
			if (view == null)
				view = inflater.Inflate (Resource.Layout.listview_item_drugstore, null, false);
			var item = list [position];
			string density = LoadingImageUtil.getDensityInformation (context);
			string path = null;
			ImageView imv_profile = view.FindViewById<ImageView> (Resource.Id.img_item);
			if (item.profilePicutre == null)
				//path = GlobalVariable.applicationPath + "/" + GlobalVariable.folderName + "/" + density + "/" + "ic_store_default.png";
				imv_profile.SetImageResource (Resource.Drawable.ic_store_default);
			else
				path = GlobalVariable.applicationPath + "/" + GlobalVariable.folderName + "/" + density + "/" + item.profilePicutre;			
			if (cancelPotentialWork (path, imv_profile) && path != null) {
				BitmapWorkerTask decoder = new BitmapWorkerTask (imv_profile, item.profilePicutre, context, path);
				decoder.Execute (1);
			}
			TextView txt_name = view.FindViewById<TextView> (Resource.Id.txt_item);
			txt_name.Text = item.name;
			TextView txt_detail = view.FindViewById<TextView> (Resource.Id.txt_item_detail);
			txt_detail.Text = item.addressDetail;
			return view;
		}

		public override int Count {
			get {
				return list.Count;
			}
		}

	
		
	}
}

