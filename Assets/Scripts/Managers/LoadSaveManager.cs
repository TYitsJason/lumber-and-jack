using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class LoadSaveManager : MonoBehaviour
{
    [XmlRoot("GameData")]
    public class GameStateData
    {
        public struct DataTransform
        {
            public float posX, posY, posZ;
            public float rotX, rotY, rotZ;
            public float scale;
        }
        // Data for player
        public class DataPlayer
        {
            public DataTransform trans;
            public int health;
            public bool hasMainAttack;
            public bool hasSecondaryAttack;
        }
        public class DataGlobal
        {
            public int clearedWave;
        }

        public DataPlayer player = new DataPlayer();
        public DataGlobal global = new DataGlobal();
    }

    public GameStateData gameState = new GameStateData();

    // Save
    public void Save(string filename = "GameData.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
        FileStream stream = new FileStream(filename, FileMode.Create);
        serializer.Serialize(stream, gameState);

        stream.Flush();
        stream.Dispose();
        stream.Close();
    }
    
    // Load
    public void Load(string fileName = "GameData.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
        FileStream stream = new FileStream(fileName, FileMode.Open);
        gameState = serializer.Deserialize(stream) as GameStateData;

        stream.Flush();
        stream.Dispose();
        stream.Close();
    }
}
