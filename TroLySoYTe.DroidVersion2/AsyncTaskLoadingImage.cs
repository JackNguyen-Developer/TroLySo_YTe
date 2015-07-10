using System;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using Android.Content;

namespace TroLySoYTe.DroidVersion2
{
	public class AsyncTaskLoadingImage
	{
		
	}

	public class AsyncDrawable:Android.Graphics.Drawables.BitmapDrawable
	{
		Context context;
		Java.Lang.Ref.WeakReference reference;

		public AsyncDrawable (BitmapWorkerTask  decoder, Bitmap bitmap) : base (bitmap)
		{
			reference = new Java.Lang.Ref.WeakReference (decoder);
		}

		public BitmapWorkerTask  getDecoder ()
		{
			return (BitmapWorkerTask)reference.Get ();
		}
	}

	public class BitmapWorkerTask  : AsyncTask
	{
		public string path;
		Bitmap bm;
		string picturename;
		Context context;
		Java.Lang.Ref.WeakReference refer;
		AsyncDrawable drawable;

		public BitmapWorkerTask (ImageView img, string picturename, Context c, string path)
		{
			context = c;
			refer = new Java.Lang.Ref.WeakReference (img);
			this.picturename = picturename;
			this.path = path;
		}

		protected override void OnPreExecute ()
		{								
			base.OnPreExecute ();
		}

		protected override Java.Lang.Object DoInBackground (params Java.Lang.Object[] @params)
		{
				
			bm = LoadingImageUtil.loadingBitmapFromPath (path, context);	
			return true;
		}

		protected override void OnPostExecute (Java.Lang.Object result)
		{					
			drawable = new AsyncDrawable (this, bm);
			if (refer != null && bm != null) {
				ImageView im = (ImageView)refer.Get ();	
				if (im != null)
					im.SetImageBitmap (drawable.Bitmap);			
			}
		}
	}
}

