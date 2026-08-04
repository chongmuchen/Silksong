using System;
using System.IO;
using System.Linq;

namespace TeamCherry.BuildBot;

public static class Helper
{
	public static string CombinePaths(string path1, params string[] paths)
	{
		if (path1 == null)
		{
			throw new ArgumentNullException("path1");
		}
		if (paths == null)
		{
			throw new ArgumentNullException("paths");
		}
		return paths.Aggregate(path1, Path.Combine);
	}

	public static bool FileOrFolderExists(string path)
	{
		if (!File.Exists(path))
		{
			return Directory.Exists(path);
		}
		return true;
	}

	public static void DeleteFileOrFolder(string path)
	{
		if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
		{
			Directory.Delete(path, recursive: true);
		}
		else
		{
			File.Delete(path);
		}
	}

	public static void CopyFileOrFolder(string src, string dest)
	{
		if ((File.GetAttributes(src) & FileAttributes.Directory) == FileAttributes.Directory)
		{
			DirectoryInfo source = new DirectoryInfo(src);
			DirectoryInfo target = (Directory.Exists(dest) ? new DirectoryInfo(dest) : Directory.CreateDirectory(dest));
			DeepCopy(source, target);
		}
		else
		{
			File.Copy(src, dest);
		}
	}

	private static void DeepCopy(DirectoryInfo source, DirectoryInfo target)
	{
		DirectoryInfo[] directories = source.GetDirectories();
		foreach (DirectoryInfo directoryInfo in directories)
		{
			DeepCopy(directoryInfo, target.CreateSubdirectory(directoryInfo.Name));
		}
		FileInfo[] files = source.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name));
		}
	}

	public static void MoveFileOrFolder(string src, string dest)
	{
		if ((File.GetAttributes(src) & FileAttributes.Directory) == FileAttributes.Directory)
		{
			Directory.Move(src, dest);
		}
		else
		{
			File.Copy(src, dest);
		}
	}
}
