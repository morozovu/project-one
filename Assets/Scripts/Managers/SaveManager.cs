using System.IO;
using UnityEngine;

public class SaveManager<T>
{
	public bool TryLoad(string name, out T data)
	{
		data = default(T);
		string fileName = GetFilePath(name);

		if (!File.Exists(fileName)) return false;

		using (StreamReader reader = new StreamReader(fileName))
		{
			string json = reader.ReadToEnd();

			data = JsonUtility.FromJson<T>(json);
		}

		return true;
	}

	public void Save(T[] data, string name)
	{
		string json = JsonHelper.ToJson(data);

		FileStream fileStream = new FileStream(GetFilePath(name), FileMode.Create);

		using (StreamWriter writer = new StreamWriter(fileStream))
		{
			writer.Write(json);
		}
	}

	private string GetFilePath(string name)
	{
		return Application.persistentDataPath + "/" + name + ".sav";
	}
}
