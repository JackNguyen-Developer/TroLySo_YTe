using System;
using System.Collections.Generic;

namespace TroLySoYTe
{
	public class District:PlaceBaseModel
	{
		public static string ID_COLUMN="id";
		public static string NAME_COLUMN="Name";
		public District()
		{
			wardChild = new List<Ward> ();	
			listNeighbor = new List<District> ();
		}
		public void addChild(Ward child)
		{
			wardChild.Add (child);
		}
//		public int id{get;set;}
//
//		public string name{get;set;}

		public List<Ward> wardChild;

		public List<Ward> WardChild {
			get 
			{
				return wardChild;
			}
			set 
			{
				wardChild = value;
			}
		}


		List<District> listNeighbor;	
				
		public List<District> ListNeighbor 
		{
			get 
			{
				return listNeighbor;
			}
			set 
			{
				listNeighbor = value;
			}
		}
	}
}

