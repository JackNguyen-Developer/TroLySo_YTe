using System;
using System.Collections.Generic;

namespace TroLySoYTe
{
	public class TypeMedical
	{
		public TypeMedical()
		{
			list_store = new List<DrugStore> ();
		}
		public int id{get;set;}
		public string name{ get; set; }
		public List<DrugStore> list_store;
		public List<DrugStore> Store
		{
			get{ return list_store;}
			set{list_store = value;}
		}
	}
}

