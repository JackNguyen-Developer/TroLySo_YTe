using System;
using Android.Widget;
using Android.Content;
using Android.Views;

namespace TroLySoYTe.DroidVersion2
{
	public class LeftMenuItem:BaseAdapter<string>
	{
		Context context;
		string[] array;
		public LeftMenuItem (Context c ,string[] a)
		{
			context = c;
			array = a;
		}

		#region implemented abstract members of BaseAdapter

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			LayoutInflater inflater = (LayoutInflater)context.GetSystemService (Context.LayoutInflaterService);
			View view = inflater.Inflate (Resource.Layout.slidingMenuItem_layout, null);
			TextView MenuName = view.FindViewById<TextView> (Resource.Id.txt_slmenu);
			ImageView menuIcon = view.FindViewById<ImageView> (Resource.Id.img_icon_slmenu);
			MenuName.Text = array [position];
			SelectIcon (position, menuIcon);
			return view;
		}
		private void SelectIcon(int position,ImageView imgv)
		{
			if (position == 0) {
				imgv.SetImageResource (Resource.Drawable.ic_map);
			} else {
				imgv.SetImageResource (Resource.Drawable.ic_intro);
			}
		}
		public override int Count {
			get {
				return array.Length;
			}
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override string this [int index] {
			get {
				return array [index];
			}
		}

		#endregion
	}
}

