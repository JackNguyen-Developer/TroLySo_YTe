using System;
using Android.Content;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace TroLySoYTe.DroidVersion2
{
	public class DataAccessLayer:IConnection
	{
		public List<DrugStore> getstorebyname (string name)
		{
			List<DrugStore> list = new List<DrugStore> ();
			var cursor = db.getstorebykeyword (name);
			if (cursor.Count > 0) {
				cursor.MoveToFirst ();
				while (!cursor.IsAfterLast) {
					list.Add (new DrugStore {
						id = cursor.GetInt (0),
						name = cursor.GetString (1),
						addressDetail = cursor.GetString (2)
					});
					cursor.MoveToNext ();
				}
				return list;
			}
			return list;
		}

		public List<Ward> GetStoreByWardId (int id)
		{
			throw new NotImplementedException ();
		}

		public List<DrugStore> GetDrugStoreByDistrict (string name)
		{
			throw new NotImplementedException ();
		}

		public List<DrugStore> GetDrugStoreByDistrictID (int id)
		{
			throw new NotImplementedException ();
		}

		public List<DrugStore> GetDrugStoreByWard (string name)
		{
			throw new NotImplementedException ();
		}


		public List<DrugStore> GetDrugStoreByStreet (string name)
		{
			throw new NotImplementedException ();
		}

		public List<DrugStore> GetDrugStoreByStreetID (int id)
		{
			throw new NotImplementedException ();
		}

		public List<DrugStore> GetDrugStoreByKeyword (string key)
		{
			List<DrugStore> list = new List<DrugStore> ();

			try {
				var cursor = db.getstorebykeyword (key);
				if (cursor.Count > 0) {
					cursor.MoveToFirst ();
					while (!cursor.IsAfterLast) {
						list.Add (new DrugStore {
							id = cursor.GetInt (0),
							name = cursor.GetString (1),
							addressDetail = cursor.GetString (2),
							latitude = cursor.GetDouble (3),
							longtitude = cursor.GetDouble (4),
							profilePicutre = cursor.GetString (5)
						});
						cursor.MoveToNext ();
					}
					return list;
				}
			} catch (Exception e) {
				return list;
			}
			return null;
		}

		public List<DrugStore> GetDrugStoreByType (string type)
		{
			throw new NotImplementedException ();
		}

		#region IConnection implementation

		DatabaseHelper db;

		public List<DrugStore> GetDrugStoreByWardID (int id)
		{
			List<DrugStore> list_store = new List<DrugStore> ();
			var cursor = db.getStoreByWardId (id);
			if (cursor.MoveToFirst ()) {
				for (int i = 0; i < cursor.Count; i++) {
					DrugStore item = new DrugStore {
						id = cursor.GetInt (0),
						name = cursor.GetString (1),
						address = new Address {
							id = cursor.GetInt (2),
							number = cursor.GetString (3),
							street = new Street {
								id = cursor.GetInt (4),
								name = cursor.GetString (5),
							},
							ward = new Ward {
								id = cursor.GetInt (6),
								name = cursor.GetString (7),
							},
							disctrict = new District {
								id = cursor.GetInt (8),
								name = cursor.GetString (9)
							}

						}
					};
					list_store.Add (item);
					cursor.MoveToNext ();
				}
				return list_store;
			}
			return list_store;
		}

		public List<District> GetAllDistrict ()
		{
			var cursor = db.abc ();
			List<District> list = new List<District> ();
			list.Add (new District {
				name = "Chọn quận",
			});
			list [0].addChild (new Ward{ name = "Chọn phường" });
			if (cursor.MoveToFirst ()) {
				for (int i = 0; i < cursor.Count; i++) {
					int id = cursor.GetInt (cursor.GetColumnIndex (District.ID_COLUMN));
					string name = cursor.GetString (cursor.GetColumnIndex (District.NAME_COLUMN));
					District item = new District{ id = id, name = name };

					var cursorWard = db.GetWardFromDistrictID (id);
					if (cursorWard.MoveToFirst ()) {						
							
						item.addChild (new Ward{ name = "Chọn phường" });
						for (int j = 0; j < cursorWard.Count; j++) {
							item.addChild (
								new Ward {
									id = cursorWard.GetInt (cursor.GetColumnIndex (Ward.ID_COLUMN)),
									name = cursorWard.GetString (cursor.GetColumnIndex (Ward.NAME_COLUMN)),
									districtParent = item
								});
							cursorWard.MoveToNext ();
						}

					} else {
						item.addChild (new Ward{ name = "Không tìm thấy" });
					}
					list.Add (item);
					cursor.MoveToNext ();
				}
				return list;
			}
			return list;
		}

		#endregion

		public DataAccessLayer (Context context)
		{
			db = new DatabaseHelper (context);
		}

		public List<DrugStore> GetDrugStoreByDistrictOrWard (string district, string ward)
		{
			List<DrugStore> list = new List<DrugStore> ();
			try {
				var cursor = db.GetDrugstoreByDistrictOrWard (district, ward);
				if (cursor.Count > 0) {
					cursor.MoveToFirst ();
					while (!cursor.IsAfterLast) {
						list.Add (new DrugStore {
							id = cursor.GetInt (0),
							name = cursor.GetString (1),
							addressDetail = cursor.GetString (2),
							latitude = cursor.GetDouble (3),
							longtitude = cursor.GetDouble (4)
						});
						cursor.MoveToNext ();
					}
					return list;
				}
				return null;
			} catch (Exception e) {
				return null;
			}
		
		}

		public List<DrugStore> GetDrugStoreByKeyword (string key, int limit)
		{
			List<DrugStore> list = new List<DrugStore> ();
			var cursor = db.getstorebykeyword (key, limit);
			if (cursor.Count > 0) {
				cursor.MoveToFirst ();
				while (!cursor.IsAfterLast) {
					list.Add (new DrugStore {
						id = cursor.GetInt (0),
						name = cursor.GetString (1),
						addressDetail = cursor.GetString (2),
						profilePicutre = cursor.GetString (3),
						latitude = cursor.GetDouble (4),
						longtitude = cursor.GetDouble (5)
					});
					cursor.MoveToNext ();
				}
				return list;
			}
			return list;
		}
	}
}

