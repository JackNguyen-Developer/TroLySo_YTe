using System;
using System.Collections.Generic;

namespace TroLySoYTe
{
	public class MainViewModel:BaseViewModel
	{
		public List<DrugStore> list_drugstore{ get; set; }

		public bool GetDrugStoreByKeyWord (string keyword)
		{	
			list_drugstore = DAL.GetDrugStoreByKeyword (keyword);
			List<DrugStore> list = DAL.GetDrugStoreByKeyword (keyword);
			return list_drugstore == null ? false : true;
		}

		public bool GetDrugStoreByDistrict (string district, string ward)
		{
			list_drugstore = DAL.GetDrugStoreByDistrictOrWard (district, ward);
			return list_drugstore == null ? false : true;
		}

		public bool GetDrugStoreByKeyWord (string keyword, int limit)
		{	
			list_drugstore = DAL.GetDrugStoreByKeyword (keyword, limit);
//			List<DrugStore> list= DAL.GetDrugStoreByKeyword(keyword);
			return list_drugstore == null ? false : true;
		}
	}
}

