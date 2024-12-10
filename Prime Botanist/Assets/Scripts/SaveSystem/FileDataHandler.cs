using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameSettings Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameSettings loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad;

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameSettings>(dataToLoad);
            } catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        return loadedData;
    }

    public void Save(GameSettings settings)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(settings, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        } catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
