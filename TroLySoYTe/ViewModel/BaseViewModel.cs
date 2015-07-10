using System;
using System.Collections.Generic;

namespace TroLySoYTe
{
	public class BaseViewModel
	{	
		public IConnection DAL=ServiceContainer.Resolve<IConnection>();
		bool isBusy=false;

		public bool IsBusy {
			get {
				return isBusy;
			}
			set{ 
				isBusy = value;
				IsBusyChanged (this, EventArgs.Empty);
			}
		}
		IConnection db=ServiceContainer.Resolve<IConnection>();
		public event EventHandler IsBusyChanged=delegate {};
	}
}

