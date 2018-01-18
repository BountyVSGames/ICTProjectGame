using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace EditorScripts
{
    public class S_LevelManagerInfoWindow : EditorWindow
    {
        [MenuItem("Project Information/For the level manager")]
        static void Init()
        {
            S_AboutTheProjectWindow Window = (S_AboutTheProjectWindow)EditorWindow.GetWindow(typeof(S_AboutTheProjectWindow), true, "For the level manager");

            Window.maxSize = new UnityEngine.Vector2(375, 800);

            Window.Show();
        }

        void OnGUI()
        {
            UnityEngine.GUILayout.Width(100);
            UnityEngine.GUILayout.Height(100);

            UnityEngine.GUILayout.Label("How to create a component", EditorStyles.boldLabel);
            UnityEngine.GUI.Label(new UnityEngine.Rect(5, 70, 700, 700), "1. Drag the S_PCComponent script on the component\n2. \n3. \n4.");
        }
    }
}
