using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace EditorScripts
{
    public class S_AboutTheProjectWindow : EditorWindow
    {
        [MenuItem("Project Information/About the project")]
        static void Init()
        {
            S_AboutTheProjectWindow Window = (S_AboutTheProjectWindow)EditorWindow.GetWindow(typeof(S_AboutTheProjectWindow), true, "About the project");

            Window.maxSize = new UnityEngine.Vector2(375, 800);

            Window.Show();
        }

        void OnGUI()
        {
            UnityEngine.GUILayout.Label("Project Planning:", EditorStyles.boldLabel);
            if(UnityEngine.GUILayout.Button("Press here"))
            {
                UnityEngine.Application.OpenURL("https://trello.com/b/qMshvgU0/planning");
            }

            UnityEngine.GUILayout.Width(100);
            UnityEngine.GUILayout.Height(100);

            UnityEngine.GUILayout.Label("How to import assests in the project:", EditorStyles.boldLabel);
            UnityEngine.GUI.Label(new UnityEngine.Rect(5, 70, 700, 700), "1. Load the project into your own folder of the project at:\n    'Assests\\Resources\\Art Assests\\%your name here%'\\...'.\n2. \n3. \n4.");
        }
    }
}