using System;
using Android.Database.Sqlite;
using Android.Content;
using System.IO;
using Android.App;
using Android.Database;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Net;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace TroLySoYTe.DroidVersion2
{
	public class DatabaseHelper:SQLiteOpenHelper
	{

		string dbName = "DrugStore.sqlite";
		Context myContext;
		string dbPath;
		SQLiteDatabase db;

		internal DatabaseHelper (Context context)
			: base (context, "DrugStore.sqlite", null, 1)
		{			
			myContext = context;
			dbPath = Path.Combine ("data/data/" + context.PackageName + "/", "DrugStore.sqlite");
			//dbPath = Path.Combine (Android.OS.Environment.DirectoryDocuments.ToString (), "DrugStore.sqlite");
			checkDB ();
			openDatabase ();
		}

		public override void OnCreate (SQLiteDatabase db)
		{
			
		}

		public void checkDB ()
		{

			if (!File.Exists (dbPath)) {				
				using (BinaryReader br = new BinaryReader (myContext.Assets.Open (dbName))) {
					using (BinaryWriter bw = new BinaryWriter (new FileStream (dbPath, FileMode.Create))) {
						byte[] buffer = new byte[2048];
						int len = 0;
						while ((len = br.Read (buffer, 0, buffer.Length)) > 0) {
							bw.Write (buffer, 0, len);
						}
					}
				}
			}		
		}

		public void openDatabase ()
		{			
			db = SQLiteDatabase.OpenOrCreateDatabase (dbPath, null);
		}

		public class Ward
		{
			public int id{ get; set; }

			public string name{ get; set; }
		}

		public ICursor abc ()
		{
			openDatabase ();
			//db.RawQuery ("Select * from district", null);
			var cursor = db.RawQuery ("SELECT id,name FROM district", null);
			return cursor;				
		}

		public ICursor getStoreByWardId (int id)
		{
			string query = "SELECT drugstore.id ,drugstore.name as storename,address.id as addressid,address.number,street.id,street.name as streetname,ward.id,ward.name as wardname,district.id,district.name as districtname " +
			               "FROM ward,address,drugstore,street,district " +
			               "where ward.id=address.ward " +
			               "and address.id=drugstore.address " +
			               "and ward.id = " + id + " and address.street=street.id " +
			               "and address.district=district.id ";
			//string query="select * from drugstore,address,street,ward,district where drugstore.address=address.id and address.street=street.id and address.ward=ward.id and address.district=district.id and ward.id =1";
			var cursor = db.RawQuery (query, null);
			return cursor;		
		}

		//		private List<string> decodeString (string mString)
		//		{
		//			List<string> listString = new List<string> ();
		//
		//
		//			string[] aString = { "a", "à", "ả", "ã", "ạ", "ă", "ắ", "ặ" + "ằ" + "ẳ", "ẵ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ" };
		//			string[] dString = { "d", "đ" };
		//			string[] eString = { "e", "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ" };
		//			string[] iString = { "i", "í", "ì", "ỉ", "ĩ", "ị" };
		//			string[] oString = { "o", "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ" };
		//			string[] uString = { "u", "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự" };
		//			string[] yString = { "y", "ý", "ỳ", "ỷ", "ỹ", "ỵ" };
		//
		//			string[] allString = {"à", "ả", "ã", "ạ", "ă", "ắ", "ặ" + "ằ" + "ẳ", "ẵ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "đ", "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ",
		//				"í", "ì", "ỉ", "ĩ", "ị", "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ",
		//				"ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự", "ý", "ỳ", "ỷ", "ỹ", "ỵ"
		//			};
		//
		//			foreach (var item in aString) {
		//				StringBuilder text = new StringBuilder (mString);
		//				text.Replace ("a", item);
		//				listString.Add (text.ToString ());
		////				if (!text.ToString ().Equals (mString)) {
		////					listString.Add (text.ToString ());
		////				} else {
		////					listString.Add (mString);
		////					break;
		////				}
		//			}
		//
		//			foreach (var item in dString) {
		//				StringBuilder text = new StringBuilder (mString);
		//				text.Replace ("d", item);
		//				listString.Add (text.ToString ());
		////				if (!text.ToString ().Equals (mString)) {
		////					listString.Add (text.ToString ());
		////				} else {
		////					listString.Add (mString);
		////					break;
		////				}
		//			}
		//			foreach (var item in eString) {
		//				StringBuilder text = new StringBuilder (mString);
		//				text.Replace ("e", item);
		//				listString.Add (text.ToString ());
		////				if (!text.ToString ().Equals (mString)) {
		////					listString.Add (text.ToString ());
		////				} else {
		////					listString.Add (mString);
		////					break;
		////				}
		//			}
		//			foreach (var item in iString) {
		//				StringBuilder text = new StringBuilder (mString);
		//				text.Replace ("i", item);
		//				listString.Add (text.ToString ());
		////				if (!text.ToString ().Equals (mString)) {
		////					listString.Add (text.ToString ());
		////				} else {
		////					listString.Add (mString);
		////					break;
		////				}
		//			}
		//			foreach (var item in oString) {
		//				StringBuilder text = new StringBuilder (mString);
		//				text.Replace ("o", item);
		//				listString.Add (text.ToString ());
		////				if (!text.ToString ().Equals (mString)) {
		////					listString.Add (text.ToString ());
		////				} else {
		////					listString.Add (mString);
		////					break;
		////				}
		//			}
		//			foreach (var item in uString) {
		//				StringBuilder text = new StringBuilder (mString);
		//				text.Replace ("u", item);
		//				listString.Add (text.ToString ());
		////				if (!text.ToString ().Equals (mString)) {
		////					listString.Add (text.ToString ());
		////				} else {
		////					listString.Add (mString);
		////					break;
		////				}
		//			}
		//			foreach (var item in yString) {
		//				StringBuilder text = new StringBuilder (mString);
		//				text.Replace ("y", item);
		//				listString.Add (text.ToString ());
		////				if (!text.ToString ().Equals (mString)) {
		////					listString.Add (text.ToString ());
		////				} else {
		////					listString.Add (mString);
		////					break;
		////				}
		//			}
		//			return listString;
		//		}

		public ICursor getstorebykeyword (string key)
		{
			//List<string> decodeKey = decodeString (key);
			ICursor cursorAll = null;
			//string query = "select * from drugstore,address,street,ward,district where (drugstore.address=address.id and address.street=street.id and address.ward=ward.id and address.district=district.id ) and (  district.name like '%"+key+"%'  or  ward.name like '%"+key+"%' or drugstore.name like '%"+key+"%' or  street.name like '%"+key+"%'  )";
			//string query="SELECT * FROM ward,address WHERE   ward.id=address.ward and (ward.name like '%"+key+"%')";
			//for (int i = 0; i < decodeKey.Count; i++) {
			//	string item = decodeKey [i];
			string query = "select id,name,detail,latitude,longtitude,picture from drugstore where detail like ? or name like ? limit 10";
			//string query = "select id,name,detail,latitude,longtitude,picture from drugstore where detail like ?" + new string[+"%"+ key +"%"] + "limit 10";
			SQLiteDatabase data = db;
			var cursor = db.RawQuery (query, new string[]{ "%" + key + "%", "%" + key + "%" });
			//if (cursor.Count > 0) {
			//	cursorAll = cursor;
			//	break;
			//}

			//}
			return cursor;		
		}

		public ICursor getstorebyname (string name)
		{
			string query = "select * from drugstore where name like '%" + name + "%' limit 10";
			var cursor = db.RawQuery (query, null);
			return cursor;
		}

		void closeDatabase ()
		{
			db.Close ();
		}

		public ICursor GetWardFromDistrictID (int id)
		{
			var cursor = db.RawQuery ("SELECT id,name FROM ward where District=" + id, null);
			return cursor;		
		}

		public override void OnUpgrade (SQLiteDatabase db, int oldVersion, int newVersion)
		{			
		}

		public ICursor GetDrugstoreByDistrictOrWard (string district, string ward)
		{
			
			string query = null;
			if (ward == null)
				query = "select drugstore.id ,drugstore.name, drugstore.detail, drugstore.latitude, drugstore.longtitude " +
				" from drugstore, address, district, ward " +
				" where district.name like '%" + district + "%' " +
				" and district.id = ward.district" +
				" and address.district = district.id and address.ward = ward.id" +
				" and drugstore.address = address.id";
			else
				query = "select drugstore.id ,drugstore.name, drugstore.detail, drugstore.latitude, drugstore.longtitude " +
				" from drugstore, address, district, ward " +
				" where district.name like '%" + district + "%' " +
				" and ward.name like '%" + ward + "%' " +
				" and district.id = ward.district" +
				" and address.district = district.id and address.ward = ward.id" +
				" and drugstore.address = address.id";

			var cursor = db.RawQuery (query, null);
			if (cursor.Count > 0) {
//				cursor.MoveToFirst ();
//				while (!cursor.IsAfterLast) {
//					int id = cursor.GetInt (0);
//					cursor.MoveToNext ();
//				}
//				return null;
				return cursor;
			}
			return null;
		}

		public ICursor getstorebykeyword (string key, int limit)
		{
			//openDatabase ();
			//string query = "select * from drugstore,address,street,ward,district where (drugstore.address=address.id and address.street=street.id and address.ward=ward.id and address.district=district.id ) and (  district.name like '%"+key+"%'  or  ward.name like '%"+key+"%' or drugstore.name like '%"+key+"%' or  street.name like '%"+key+"%'  )";
			//string query="SELECT * FROM ward,address WHERE   ward.id=address.ward and (ward.name like '%"+key+"%')";
			string query = "select id,name,detail,picture,latitude,longtitude from drugstore where detail like '%" + key + "%' or name like '%" + key + "%' limit " + limit;
			SQLiteDatabase data = db;
			var cursor = db.RawQuery (query, null);
			return cursor;		
		}
	}
}

