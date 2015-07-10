using System;
using Android.Content;
using Android.Graphics.Drawables;

namespace TroLySoYTe.DroidVersion2
{
	public class RunnableAnonymousInnerClassHelper : Java.Lang.Object, Java.Lang.IRunnable
	{
		private readonly Context outerInstance;


		public RunnableAnonymousInnerClassHelper (Context outerInstance, AnimationDrawable animDrawable)
		{
			this.outerInstance = outerInstance;

		}

		public void Run ()
		{
			Console.WriteLine ("*** Runnable starting");     // <-- does not show on Output panel
		}
	}
}

