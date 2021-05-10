using UnityEditor;
using UnityEngine;

public class ScenePhysicsTool : EditorWindow
{

    bool state;

    private void OnGUI()
    {
        state = GUILayout.RepeatButton("Run Physics");
        if(Event.current.type == EventType.Repaint)
        {
            Debug.Log("OnGUI: " + state);
        }

        if(!state)
        {
            Physics.autoSimulation = true;
        }
    }

    private void Update()
    {
        if(state)
        {
            Debug.Log("Doing Physics..");
            Physics.autoSimulation = false;
            Physics.Simulate(Time.fixedDeltaTime);
        }
    }

    private void StepPhysics()
    {
        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.autoSimulation = true;
    }

    [MenuItem("Tools/Scene Physics")]
    private static void OpenWindow()
    {
        GetWindow<ScenePhysicsTool>(false, "Physics", true);
    }
}