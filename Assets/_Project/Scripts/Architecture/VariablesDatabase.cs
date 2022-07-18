using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class EditorVariable<T>
{
    public string label;
    public TrackedVariable<T> variable;

    public EditorVariable(string p_label, TrackedVariable<T> p_variable)
    {
        label = p_label;
        variable = p_variable;
    }
}

public class EditorVariableData
{
    public static List<EditorVariableData> EditorDataList = new List<EditorVariableData>();

    public string label;
    public List<EditorVariable<float>> trackedFloats = new List<EditorVariable<float>>();

    public EditorVariableData()
    {
        EditorDataList.Add(this);
    }
}
#endif

/// <summary>
/// Variables that are used outside of the target instance scope.
/// Ex.: HealthPoints needs to be accessed by the Player and HUD, but HUD must not depend on the Player existing to be tested.
/// </summary>
//Needs to being used so it shows up in the UIToolkit Window menu.
public static class VariablesDatabase
{
    public class PlayerData
    {
        /// <summary>
        /// Only player is allowed to edit the value of this variable.
        /// </summary>
        public TrackedVariable<float> HealthPoints = new TrackedVariable<float>(0);
        public TrackedVariable<float> ManaPoints = new TrackedVariable<float>(0);

        public PlayerData(int p_id)
        {
            Debug.Log("PlayerData " + p_id);
            #if UNITY_EDITOR
            EditorVariableData __editorData = new EditorVariableData();

            __editorData.label = "Player " + p_id;
            __editorData.trackedFloats.Add(new EditorVariable<float>("HealthPoints", HealthPoints));
            __editorData.trackedFloats.Add(new EditorVariable<float>("ManaPoints", ManaPoints));
            Debug.Log("Construct Player Data");
            #endif
        }
    }

    public static PlayerData[] Player = new PlayerData[2] { new PlayerData(0), new PlayerData(1) };
}