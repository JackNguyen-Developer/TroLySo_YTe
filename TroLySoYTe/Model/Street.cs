using System;
using System.Collections.Generic;

namespace TroLySoYTe
{
	public class Street:PlaceBaseModel
	{
		public Street ()
		{
			listWard = new List<Ward> ();
		}
		public void addWard(Ward ward)
		{
			listWard.Add (ward);
		}
//		public int id{ get; set; }
//
//		public string name{ get; set; }

		public List<Ward> listWard;

		public List<Ward> ListWard {
			get {
				return listWard;
			}
			set{ listWard = value; }
		}
	}
}

