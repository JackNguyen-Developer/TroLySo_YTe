using System;

namespace TroLySoYTe
{
	public class Address
	{
		public Address ()
		{
		}
		public int id{ get; set; }
		public string number{get;set;}
		public Street street{get;set;}
		public Ward ward{get;set;}
		public District disctrict{get;set;}
	}
}

