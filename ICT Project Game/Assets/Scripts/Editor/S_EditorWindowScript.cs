using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorScripts
{
    public class EditorWindowScript : EditorWindow
    {
        private EditorWindowScript m_Window;

        [MenuItem("Project Information/About the project")]
        static void Init()
        {
            EditorWindowScript Window = (EditorWindowScript)EditorWindow.GetWindow(typeof(EditorWindowScript), true, "About the project");

            Window.maxSize = new Vector2(375, 800);

            Window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Project Planning:", EditorStyles.boldLabel);
            if(GUILayout.Button("Press here"))
            {
                Application.OpenURL("https://trello.com/b/qMshvgU0/planning");
            }

            GUILayout.Width(100);
            GUILayout.Height(100);

            GUILayout.Label("How to import assests in the project:", EditorStyles.boldLabel);
            GUI.Label(new Rect(5, 70, 700, 700), "1. Load the project into your own folder of the project at:\n    'Assests\\Resources\\Art Assests\\%your name here%'\\...'.\n2. \n3. \n4.");
        }
    }
}