using System;
using Android.Graphics;
using System.IO;
using Android.Content;
using Android.OS;

namespace TroLySoYTe.DroidVersion2
{
	public static class LoadingImageUtil
	{
		public static Bitmap loadingBitmapFromPath (string path, Context c)
		{
			BitmapFactory.Options bmOptions = new BitmapFactory.Options ();
			bmOptions.InJustDecodeBounds = true;
			BitmapFactory.DecodeResource (c.Resources, Resource.Drawable.ic_store_default, bmOptions);
			bmOptions.InSampleSize = calculateInputSample (bmOptions, 200, 200);
			bmOptions.InJustDecodeBounds = false;
			Bitmap bitmap = null;
			if (File.Exists (path))
				bitmap = BitmapFactory.DecodeFile (path, bmOptions);
			return bitmap;
		}

		public static Bitmap loadingBitmapFromPath (string path, Context c, int reqheight, int reqwidth)
		{
			BitmapFactory.Options bmOptions = new BitmapFactory.Options ();
			bmOptions.InJustDecodeBounds = true;
			BitmapFactory.DecodeResource (c.Resources, Resource.Drawable.ic_store_default, bmOptions);
			bmOptions.InSampleSize = calculateInputSample (bmOptions, reqheight, reqwidth);
			bmOptions.InJustDecodeBounds = false;
			Bitmap bitmap = null;
			if (File.Exists (path))
				bitmap = BitmapFactory.DecodeFile (path, bmOptions);
			return bitmap;
		}

		public static int calculateInputSample (BitmapFactory.Options options, int reqHeight, int reqWidth)
		{
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inputSample = 1;
			while (reqHeight < outHeight || reqWidth < outWidth) {
				int halfHeight = outHeight / 2;
				int halfWidth = outWidth / 2;
				while ((halfHeight / 2) > reqHeight || (halfWidth / 2) > reqWidth) {
					inputSample *= 2;
				}
			}
			return inputSample;
		}

		public static string getDensityInformation (Context context)
		{
			int density = (int)context.Resources.DisplayMetrics.DensityDpi;
			string densityDpi = "";
			if (density == 240)
				densityDpi = "drawable-hdpi";
			else if (density == 360)
				densityDpi = "drawable-xhdpi";
			else if (density == 480)
				densityDpi = "drawable-xxhdpi";
			else if (density == 600)
				densityDpi = "drawable-xxxhdpi";
			else
				densityDpi = "drawable-mdpi";
					
			return densityDpi;
		}
	}
}

