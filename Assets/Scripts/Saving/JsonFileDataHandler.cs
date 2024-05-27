using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Saving
{
	public class JsonFileDataHandler
	{
		/// <summary>
		/// This class is used by PersistentDataManager to save/load and encrypt/decrypt data for the saving System.
		/// </summary>

		private string _dataDirPath = "";
		private string _dataFileName = "";

		// Encryption
		private bool _useEncryption = false;
		private readonly string _encryptionKey = "word"; // This is a password used for the encryption

		// MARK: PUBLIC:

		public JsonFileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
		{
			_dataDirPath = dataDirPath;
			_dataFileName = dataFileName;
			_useEncryption = useEncryption;
		}

		public SaveData Load()
		{
			// use Path.Combine to account for different OS's having different path separators.
			// Windows: Application.persistentDataPath points to C:\Users\<user>\AppData\LocalLow\<company name>.
			// MAC OS: ~/Library/Application Support/company name/product name (originally hidden)
			string fullPath = System.IO.Path.Combine(_dataDirPath, _dataFileName);
			SaveData loadedData = null;

			if (File.Exists(fullPath))
			{
				try
				{
					string dataToLoad = "";

					using (FileStream stream = new FileStream(fullPath, FileMode.Open))
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							dataToLoad = reader.ReadToEnd();
						}
					}

					// optionally decrypt the data
					if (_useEncryption)
					{
						dataToLoad = EncryptDecrypt(dataToLoad);
					}

					// Deserialize the data from Json back into the C# object
					loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);
				}
				catch (Exception e)
				{
					Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
				}
			}
			return loadedData;
		}

		public void Save(SaveData data)
		{
			// use Path.Combine to account for different OS's having different path separators.
			string fullPath = System.IO.Path.Combine(_dataDirPath, _dataFileName);
			try
			{
				// create the directory that the file will be written to if it doesn't already exist
				Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

				// Serialize the C# game data object into JSON
				string dataToStore = JsonUtility.ToJson(data, true);

				// optionally encrypt the data
				if (_useEncryption)
				{
					dataToStore = EncryptDecrypt(dataToStore);
				}

				// Write the serialized data to the file
				using (FileStream stream = new FileStream(fullPath, FileMode.Create))
				{
					using (StreamWriter writer = new StreamWriter(stream))
					{
						writer.Write(dataToStore);
						// Debug.Log("PersistentDataManager: Saved Data");
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
			}
		}

		// MARK: PRIVATE:

		// The below is a simple implementation of XOR encryption
		private string EncryptDecrypt(string data)
		{
			string modifiedData = "";
			for (int i = 0; i < data.Length; i++)
			{
				modifiedData += (char)(data[i] ^ _encryptionKey[i % _encryptionKey.Length]);
			}

			return modifiedData;
		}
	}
}
