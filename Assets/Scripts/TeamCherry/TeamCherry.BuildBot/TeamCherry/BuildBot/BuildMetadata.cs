using System;
using System.IO;
using UnityEngine;

namespace TeamCherry.BuildBot
{

[Serializable]
public class BuildMetadata
{
	[SerializeField]
	private string branchName;

	[SerializeField]
	private string revision;

	[SerializeField]
	private long commitTime;

	[SerializeField]
	private string machineName;

	[SerializeField]
	private long buildTime;

	private static bool _didLoad;

	private static BuildMetadata _embedded;

	public const string EMBEDDED_FILE_NAME = "BuildMetadata.json";

	public string BranchName => branchName;

	public string Revision => revision;

	public DateTime CommitTime => DateTime.FromBinary(commitTime);

	public string MachineName => machineName;

	public DateTime BuildTime => DateTime.FromBinary(buildTime);

	public static BuildMetadata Embedded
	{
		get
		{
			if (_didLoad)
			{
				return _embedded;
			}
			_didLoad = true;
			try
			{
				BuildMetadata buildMetadata = new BuildMetadata();
				JsonUtility.FromJsonOverwrite(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "BuildMetadata.json")), (object)buildMetadata);
				_embedded = buildMetadata;
			}
			catch (DirectoryNotFoundException)
			{
			}
			catch (FileNotFoundException)
			{
			}
			catch (Exception ex3)
			{
				Debug.LogException(ex3);
			}
			return _embedded;
		}
	}

	public static BuildMetadata Create(string branchName, string revision, DateTime commitTime, string machineName, DateTime buildTime)
	{
		return new BuildMetadata
		{
			branchName = branchName,
			revision = revision,
			commitTime = commitTime.ToBinary(),
			machineName = machineName,
			buildTime = buildTime.ToBinary()
		};
	}
}
}
