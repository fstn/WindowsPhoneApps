using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyToolkit.Storage
{
	public static class StorageFolderExtensions
	{
		[DebuggerHidden]
		public static async Task<StorageFolder> GetOrCreateFolderAsync(this StorageFolder folder, string folderName)
		{
			try { return await folder.CreateFolderAsync(folderName); } catch { }
			return await folder.GetFolderAsync(folderName);
		}

		[DebuggerHidden]
		public static async Task<StorageFile> GetFileSaveAsync(this StorageFolder folder, string fileName)
		{
			try { return await folder.GetFileAsync(fileName); }
			catch { }
			return null; 
		}
	}
}