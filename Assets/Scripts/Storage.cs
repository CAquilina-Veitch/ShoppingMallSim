using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class Storage : MonoBehaviour
{
    public Progress p;

    bool DontSave;

    string saveFileLocation;

    public bool FileExists()
    {
        saveFileLocation = Application.persistentDataPath + "/gameProgress.data";
        return File.Exists(saveFileLocation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(123);
            writeFile();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log(456);
            readFile();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetSaveFile();
        }
        
        
    }
    


    void Awake()
    {
        saveFileLocation = Application.persistentDataPath + "/gameProgress.data";
        Debug.Log(saveFileLocation);

        readFile();
        StartCoroutine(AutoSave());
    }


    public void ResetSaveFile()
    {
        DontSave = true;
        if (File.Exists(saveFileLocation))
        {
            File.Delete(saveFileLocation);

            

            Debug.Log("Save file reset");
        }
        else
        {
            Debug.Log("Save file not found. No need.");
        }
        SceneManager.LoadScene(0);
    }


    void readFile()
    {
        Debug.LogWarning("Started reading");
        if (File.Exists(saveFileLocation))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(saveFileLocation, FileMode.Open);
            stream.Seek(0, SeekOrigin.Begin);

            ProgressData data = formatter.Deserialize(stream) as ProgressData;

            stream.Close();

            p.LoadProgress(data);
            Debug.LogWarning("Stopped Reading");
        }
        else
        {
            Debug.LogError("Error: Save file not found in " + saveFileLocation);
        }
    }

    void writeFile()
    {
        Debug.LogWarning("Started Writing");
        p.updateProgress();

        BinaryFormatter formatter = new BinaryFormatter();


        FileStream stream = new FileStream(saveFileLocation, FileMode.Create);

        ProgressData pData = new ProgressData(p);

        formatter.Serialize(stream, pData);
        stream.Close();
        Debug.LogWarning("Stopped Writing");
    }
    private void OnDisable()
    {
        Debug.LogWarning("DISABLED");
        if (!DontSave)
        {
            writeFile();
        }


    }
    IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(30);
        writeFile();
        StartCoroutine(AutoSave());
    }

}

