using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using UnityEngine.Playables;


using UnityEditor.U2D.Animation;
using UnityEngine.TextCore.Text;

public class Storage : MonoBehaviour
{
    public Progress p;

    



    string saveFileLocation;



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
        
        
    }
    


    void Awake()
    {
        saveFileLocation = Application.persistentDataPath + "/gameProgress.data";
        Debug.Log(saveFileLocation);

        readFile();
        StartCoroutine(AutoSave());
        //data.Clear()
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

            p.LoadTo(data);
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


        FileStream stream = new FileStream(saveFileLocation, FileMode.OpenOrCreate);

        ProgressData pData = new ProgressData(p);

        formatter.Serialize(stream, pData);
        stream.Close();
        Debug.LogWarning("Stopped Writing");
    }
    private void OnDisable()
    {
       
        Debug.LogWarning("DISABLED");
        writeFile();
    }
    IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(30);
        writeFile();
        StartCoroutine(AutoSave());
    }

}

