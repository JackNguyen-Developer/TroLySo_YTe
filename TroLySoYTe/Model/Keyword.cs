using System;
using System.Collections.Generic;

namespace TroLySoYTe
{
	public class Keyword
	{
		public Keyword ()
		{
			listDrugStore = new List<DrugStore> ();
		}
		public int id{ get; set; }
		public string keyword{get;set;}
		public List<DrugStore> listDrugStore;

		public List<DrugStore> ListDrugStore {
			get {
				return listDrugStore;
			}
			set{
				listDrugStore = value;
			}
		}

		public void addDrugStore(DrugStore store)
		{
			listDrugStore.Add (store);
		}
	}
}

