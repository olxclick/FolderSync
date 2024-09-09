using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;

class FolderSync
{
    private static StreamWriter logWriter;

	//retrive and proccess arguments ++ initializing logging ++ calling the method to sync the folders
	//"C:\Users\joaol\Desktop\Stuff\" 
	static void Main(string[] args)
	{
		if (args.Length == 4)
		{
			string sourcePath = args[0];
			string replicaPath = args[1];
			string logFilePath = args[3];
			if (!int.TryParse(args[2], out int syncInterval))
			{
				syncInterval = ValidateNumber();
			}

			//logging setup
			logWriter = new StreamWriter(logFilePath, true);
			logWriter.AutoFlush = true;
			Console.WriteLine("Starting synchronization...");
			logWriter.WriteLine("Starting synchronization...");

			//synchronization on
			while (true)
			{
				SyncFolders(sourcePath, replicaPath);
				Thread.Sleep(syncInterval * 1000);
			}
		}
		else
		{
			Console.WriteLine("Please provide all necessary inputs: <source path> <replica path> <interval seconds> <log file>");
			return;
		}
	}

    //method to synchronize folders given
	static void SyncFolders(string source, string replica)
	{
		try
		{
			if (!Directory.Exists(source))
			{
				Log($"Source folder does not exist: {source}");
				return;
			}

			if (!Directory.Exists(replica))
			{
				Log($"Replica folder does not exist. Creating: {replica}");
				Directory.CreateDirectory(replica);
			}

			//copy all new and modded files from source to replica
			foreach (string sourceFilePath in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
			{
				string relativePath = Path.GetRelativePath(source, sourceFilePath);
				string replicaFilePath = Path.Combine(replica, relativePath);

				if (!File.Exists(replicaFilePath) || !FilesAreEqual(sourceFilePath, replicaFilePath))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(replicaFilePath));
					File.Copy(sourceFilePath, replicaFilePath, true);
					Log($"Copied file: {sourceFilePath} to {replicaFilePath}");
				}
			}

			//remove files in replica that are not in source
			foreach (string replicaFilePath in Directory.GetFiles(replica, "*", SearchOption.AllDirectories))
			{
				string relativePath = Path.GetRelativePath(replica, replicaFilePath);
				string sourceFilePath = Path.Combine(source, relativePath);

				if (!File.Exists(sourceFilePath))
				{
					File.Delete(replicaFilePath);
					Log($"Deleted file: {replicaFilePath}");
				}
			}

			//remove dir's from replica that are not in source
			foreach (string replicaDir in Directory.GetDirectories(replica, "*", SearchOption.AllDirectories))
			{
				string relativePath = Path.GetRelativePath(replica, replicaDir);
				string sourceDir = Path.Combine(source, relativePath);

				if (!Directory.Exists(sourceDir))
				{
					Directory.Delete(replicaDir, true);
					Log($"Deleted directory: {replicaDir}");
				}
			}
		}
		catch (Exception ex)
		{
			Log($"Error: {ex.Message}");
		}
	}

	//validation for number of interval between synchronizations (seconds)
    public static int ValidateNumber()
	{
		int result;
		string input;

		while (true)
		{
			Console.WriteLine("Please provide a valid number: ");
			input = Console.ReadLine();

			if (int.TryParse(input, out result))
			{
				return result;
			}
			else
			{
				Console.WriteLine("Input is not a valid number.");
			}
		}
	}

    //logging method (console and file)
	static void Log(string message)
	{
		string logMessage = $"{DateTime.Now} - {message}";
		Console.WriteLine(logMessage);
		logWriter.WriteLine(logMessage);
	}

	//use of md5 to check files
	static bool FilesAreEqual(string filePath1, string filePath2)
	{
		using (var md5 = MD5.Create())
		{
			using (var stream1 = File.OpenRead(filePath1))
			using (var stream2 = File.OpenRead(filePath2))
			{
				byte[] hash1 = md5.ComputeHash(stream1);
				byte[] hash2 = md5.ComputeHash(stream2);
				return hash1.SequenceEqual(hash2);
			}
		}
	}
}
