using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Helper : EditorWindow
{
    [MenuItem("Helper/Open C# Projcet", false, 11)]
    static void OpenCSharpProject()
    {
        EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
    }
}
