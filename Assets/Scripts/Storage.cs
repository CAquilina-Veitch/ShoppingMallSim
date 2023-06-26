using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using UnityEngine.Playables;

public class Storage : MonoBehaviour
{
    public Wallet w;

    public SaveData saveData;



    string saveFileLocation;
    /// TO STORE:
    /// 
    /// dictionary of business type
    ///    > Workers?
    /// 
    /// money, premium currency
    /// 
    /// time
    /// 
    /// 


    void Set(int i)
    {
        if (i == 1)
        {
            w.Currency++;
        }
        else
        {
            w.Premium++;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //data.Add("YourKeyName", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Set(1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Set(2);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            writeFile();
        }
        
        
    }
    


    void Awake()
    {
        saveFileLocation = Application.persistentDataPath + "/gameProgress.data";
        Debug.Log(saveFileLocation);
        //LoadUpgradeData();
        readFile();
        //WARNING! data.Clear() deletes EVERYTHING
        //data.Clear();
        //SaveData();
    }



    // Create a single FileStream to be overwritten as needed in the class.
    FileStream dataStream;

    // Create a single BinaryFormatter to be used across methods.
    BinaryFormatter converter = new BinaryFormatter();


    void readFile()
    {
        // Does the file exist?
        if (File.Exists(saveFileLocation))
        {
            // Create a FileStream connected to the saveFile.
            // Set to the file mode to "Open".
            dataStream = new FileStream(saveFileLocation, FileMode.Open);

            // Serialize GameData into binary data 
            //  and add it to an existing input stream.
            converter.Serialize(dataStream, saveData);

            // Close the stream
            dataStream.Close();
        }
        w.Currency = saveData.money[0];
        w.Premium = saveData.money[1];
    }
    void writeFile()
    {
        // Create a FileStream connected to the saveFile path.
        // Set the file mode to "Create".
        dataStream = new FileStream(saveFileLocation, FileMode.OpenOrCreate);

        // Deserialize binary data 
        //  and convert it into GameData, saving it as part of gameData.
        saveData = converter.Deserialize(dataStream) as SaveData;

        // Close stream.
        dataStream.Close();
    }

/*





public void LoadData()
    {
        //this loads the data
        data = DeserializeData<Dictionary<string, int>>("PleaseWork.save");
    }

    public void SaveData()
    {
        //this saves the data
        SerializeData(data, "PleaseWork.save");
    }
    public static void SerializeData<T>(T data, string path)
    {
        //this is just magic to save data.
        //if you're interested read up on serialization
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(fs, data);
            Debug.Log("Data written to " + path + " @ " + DateTime.Now.ToShortTimeString());
        }
        catch (SerializationException e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            fs.Close();
        }
    }

    public static T DeserializeData<T>(string path)
    {
        //this is the magic that deserializes the data so we can load it
        T data = default(T);

        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                data = (T)formatter.Deserialize(fs);
                Debug.Log("Data read from " + path);
            }
            catch (SerializationException e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                fs.Close();
            }
        }
        return data;
    }*/



}

