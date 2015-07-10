using System;
using System.IO;
using System.IO.Compression;
using Android.Content;

namespace TroLySoYTe.DroidVersion2
{
	public class ZipHelper
	{
		string zipFile = "zipfile.zip";
		string applicationPath = "data/data/trolysoyte.droidversion2";
		string extractPath;
		Context context;

		public ZipHelper (Context c)
		{			
			//applicationPath = Android.OS.Environment.ExternalStorageDirectory.Path;
			this.extractPath = applicationPath + "/" + zipFile;
			context = c;
		}

		public void copyFromAssert ()
		{
			if (!File.Exists (extractPath)) {				
				using (BinaryReader br = new BinaryReader (context.Assets.Open (zipFile))) {
					using (BinaryWriter bw = new BinaryWriter (new FileStream (extractPath, FileMode.Create))) {
						byte[] buffer = new byte[2048];
						int len = 0;
						while ((len = br.Read (buffer, 0, buffer.Length)) > 0) {
							bw.Write (buffer, 0, len);
						}
					}
				}
			}		
		}

		public void Unzip ()
		{			
			using (ZipArchive archive = ZipFile.OpenRead (extractPath)) {
				foreach (ZipArchiveEntry entry in archive.Entries) {					
					if (entry.FullName.EndsWith ("/", StringComparison.OrdinalIgnoreCase)) {
						FileInfo file = new FileInfo (Path.Combine (applicationPath, entry.FullName));
						file.Directory.Create ();
					} else {
						if (File.Exists (applicationPath + "/" + entry.FullName))
							File.Delete (applicationPath + "/" + entry.FullName);
						entry.ExtractToFile (applicationPath + "/" + entry.FullName);
					}
						
				}
			}
		}
	}
}

