using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TroLySoYTe
{
	public interface IConnection
	{
		List<District> GetAllDistrict ();

		List<Ward> GetStoreByWardId (int id);

		List<DrugStore> GetDrugStoreByDistrict (string name);

		List<DrugStore> GetDrugStoreByDistrictID (int id);

		List<DrugStore> GetDrugStoreByWard (string name);

		List<DrugStore> GetDrugStoreByWardID (int id);

		List<DrugStore> GetDrugStoreByStreet (string name);

		List<DrugStore> GetDrugStoreByStreetID (int id);

		List<DrugStore> GetDrugStoreByKeyword (string key);

		List<DrugStore> GetDrugStoreByType (string type);

		List<DrugStore> GetDrugStoreByDistrictOrWard (string district, string ward);

		List<DrugStore> GetDrugStoreByKeyword (string key, int limit);
	}
}

