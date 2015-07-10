using System;
using System.Collections.Generic;

namespace TroLySoYTe
{
	public class DrugStore
	{
		public DrugStore ()
		{
			listKeyword = new List<Keyword> ();
			listType = new List<TypeMedical> ();
		}
		public int id{get;set;}
		public string name{get;set;}
		public string addressDetail{get;set;}
		public  Address address{get;set;}
		public double latitude{get;set;}
		public double longtitude{get;set;}
		public List<Keyword> listKeyword;
		public string profilePicutre{ get; set; }	
		List<TypeMedical> listType;

		public List<TypeMedical> ListType 
		{
			get 
			{
				return listType;
			}
			set
			{ 
				listType = value; 
			}
		}


		public List<Keyword> ListKeyword {
			get {
				return listKeyword;
			}
			set{
				listKeyword = value;
			}
		}
		public void addkeyword(Keyword key)
		{
			listKeyword.Add (key);
		}
	}
}

