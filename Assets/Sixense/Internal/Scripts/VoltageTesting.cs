using UnityEngine;
using System.Collections.Generic;

public class VoltageTesting : MonoBehaviour
{
    class VoltageRecord
    {
        public float last = 0;
        public Dictionary<float, System.DateTime> history = new Dictionary<float, System.DateTime>(100);
    }
    Dictionary<int, VoltageRecord> m_historyVoltage = new Dictionary<int, VoltageRecord>();

	void Start () 
    {
	
	}
	
	void Update () 
    {
        foreach (var h in m_historyVoltage)
        {
            var c = SixenseCore.Device.GetTrackerByIndex(h.Key);

            c.UpdateInfo();

            if (c.BatteryVoltage < h.Value.last)
            {
                h.Value.last = c.BatteryVoltage;
                h.Value.history[h.Value.last] = System.DateTime.Now;
                Debug.Log("Voltage Change (" + h.Key + "): " + h.Value.last + " V at " + h.Value.history[h.Value.last]);
            }
        }
	}

    void OnGUI()
    {
        float scale = Mathf.Max(Screen.height, Screen.width) / 1000.0f;
        int fontsize = Mathf.FloorToInt(13.0f * scale);
        GUI.skin.label.fontSize = fontsize;
        GUI.skin.toggle.fontSize = fontsize;
        GUI.skin.textField.fontSize = fontsize;
        GUI.skin.textArea.fontSize = fontsize;
        GUI.skin.button.fontSize = fontsize;
        

        for (int i = 0; i < SixenseCore.Device.MaxNumberTrackers; i++)
        {
            var con = SixenseCore.Device.GetTrackerByIndex(i);
           
            if (m_historyVoltage.ContainsKey(i))
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Recording Voltage Drain "+ i);

                if (GUILayout.Button("Stop") || con.ExternalPower)
                    m_historyVoltage.Remove(i);

                GUILayout.EndHorizontal();

                foreach (var v in m_historyVoltage[i].history.Keys)
                {
#if !WINDOWS_UWP
                    GUILayout.Label(v + " V at " + m_historyVoltage[i].history[v].ToLongTimeString());
#endif
                }

                if (GUILayout.Button("Write Log"))
                    WritePowerLog(i);
            }
            else if(con.Connected && !con.ExternalPower)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Record Voltage Drain " + i))
                {
                    m_historyVoltage[i] = new VoltageRecord();
                    m_historyVoltage[i].last = con.BatteryVoltage;
                    m_historyVoltage[i].history.Add(con.BatteryVoltage, System.DateTime.Now);
                    Debug.Log("Initial Voltage (" + i + "): " + con.BatteryVoltage + " V at " + System.DateTime.Now);
                }
            }
        }
    }

    void WritePowerLog(int index)
    {
        var history = m_historyVoltage[index].history;

        string[] lines = new string[history.Count];

        int l = 0;
        foreach (float volt in history.Keys)
        {
            System.DateTime time = history[volt];

            lines[l++] =
                "controller: " + index +
                " voltage: " + volt +
                " day: " + time.Day +
                " hour: " + time.Hour +
                " minute: " + time.Minute +
                " second: " + time.Second;
        }

#if !WINDOWS_UWP
        System.IO.File.WriteAllLines(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) +
            "\\SixenseVoltageLog" + index + ".txt", lines);
#endif
    }
}
