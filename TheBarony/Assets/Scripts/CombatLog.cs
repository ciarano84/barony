using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CombatLog : MonoBehaviour
{
    static string logFilePath;
    
    // Start is called before the first frame update
    void Start()
    {
        logFilePath = Application.persistentDataPath + "/combatLog.txt";

        if (File.Exists(logFilePath))
        {
            try
            {
                File.Delete(logFilePath);
            }
            catch
            {
                Debug.LogError("Cannot delate log file!");
            }
        }
    }

    public static void UpdateCombatLog(string logEntry)
    {
        try
        {
            StreamWriter fileWriter = new StreamWriter(logFilePath, true);

            fileWriter.Write(logEntry);
            fileWriter.Write("\r\n");
            fileWriter.Close();
        }
        catch
        {
            Debug.LogError("Cannot write into the log file!");
        }
    }
}
