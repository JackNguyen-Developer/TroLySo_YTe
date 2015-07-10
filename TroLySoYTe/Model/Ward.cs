using System;
using System.Collections.Generic;

namespace TroLySoYTe
{
	public class Ward:PlaceBaseModel
	{	
		public static string ID_COLUMN="id";
		public static string NAME_COLUMN="Name";
		public static string DISTRICT_COLUMN="District";
		public Ward()
		{
			listStreet = new List<Street> ();
		}
		public void addStreet(Street street)
		{
			listStreet.Add (street);
		}
//		public int id{ get; set; }
//		public string name{get;set;}
		public District districtParent{ get; set; }// one to many District
		public List<Street> listStreet;// many to many Street
		public List<Street> ListStreet 
		{
			get 
			{
				return listStreet;
			}
			set
			{ 
				listStreet = value;
			}
		}
	}
}

