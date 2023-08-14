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
        if (Input.GetKeyDown(KeyCode.LeftCurlyBracket))
        {
            writeFile();
        }
        if (Input.GetKeyDown(KeyCode.RightCurlyBracket))
        {
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
        if (File.Exists(saveFileLocation))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFileLocation, FileMode.Open);

            ProgressData data = formatter.Deserialize(stream) as ProgressData;

            stream.Close();

            p.LoadTo(data);
        }
        else
        {
            Debug.LogError("Error: Save file not found in " + saveFileLocation);
        }
    }

    void writeFile()
    {
        p.updateProgress();

        BinaryFormatter formatter = new BinaryFormatter();


        FileStream stream = new FileStream(saveFileLocation, FileMode.OpenOrCreate);

        ProgressData pData = new ProgressData(p);

        formatter.Serialize(stream, pData);
        stream.Close();
    }
    private void OnDisable()
    {
        writeFile();
    }
    IEnumerator AutoSave()
    {
        yield return new WaitForSeconds(30);
        writeFile();
        StartCoroutine(AutoSave());
    }

}

