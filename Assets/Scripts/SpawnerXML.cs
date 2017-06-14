using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[RequireComponent(typeof(Spawner))]
public class SpawnerXML : MonoBehaviour
{
    // Stores individual data associated with each spawned object
    public class SpawnerData
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    [XmlRoot]
    public class XMLContainer
    {
        [XmlArray]
        public SpawnerData[] spawnerDataArray;
    }

    public string fileName = "DefaultFileName";

    Spawner spawner;
    string fullPath;
    private XMLContainer xmlContainer;

    void Awake()
    {
        // SET spawner to spawner Component
        spawner = GetComponent<Spawner>();
    }

    void Start()
    {
        // SET fullPath to Application.dataPath + "/" + filename + ".xml"
        fullPath = Application.dataPath + "/" + fileName + ".xml";
        // IF file exists
        if (File.Exists(fullPath))
        {
            //      SET xmlContainer to Load(fullPath)
            xmlContainer = Load(fullPath);
            //      CALL Apply
            Apply();
        }
    }


    // Load XMLContainer from path (NOTE: only run if the file defs exists)
    XMLContainer Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(XMLContainer));

        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as XMLContainer;
        }
    }

    // applies the saved data to the scene (using Spawner)
    void Apply()
    {
        // SET data to xmlContainer.data
        SpawnerData[] data = xmlContainer.spawnerDataArray;
        // FOR i = 0 to data.length
        for (int i = 0; i < data.Length; i++)
        {
            //  SET d to data[i]
            SpawnerData d = data[i];
            //  CALL spawner.Spawn() and pass d.position, d.rotation
            spawner.Spawn(d.position, d.rotation);
        }
    }

    // Saves whatever data value is to an XML file
    public void Save()
    {
        // SET xmlContainer to new XMLContainer
        xmlContainer = new XMLContainer();
        // SET objects to objects in spawner
        List<GameObject> objects = spawner.objects;
        // SET xmlContainer.data to new SpawnerData[objects.count]
        xmlContainer.spawnerDataArray = new SpawnerData[objects.Count];
        // FOR i = 0 to objects.count
        for (int i = 0; i < objects.Count; i++ )
        {
        //      SET data to new SpawnerData
            SpawnerData data = new SpawnerData();
        //      SET item to objects[i]
            GameObject item = objects[i];
        //      SET data position to items position
            data.position = item.transform.position;
        //      SET data rotation to items rotation
            data.rotation = item.transform.rotation;
        //      SET xmlContainer.data[i] = data
            xmlContainer.spawnerDataArray[i] = data;
        }

        // CALL SaveToPath(fullPath)
        SaveToPath(fullPath);
    }

    // Saves XML instance to a file as an XML
    void SaveToPath(string path)
    {
        // Create a serializer of type XMLContainer
        XmlSerializer serializer = new XmlSerializer(typeof(XMLContainer));
        // open a file stream at path using Create file mode
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            // serialize stream to data
            serializer.Serialize(stream, xmlContainer);
        }
    }
}
