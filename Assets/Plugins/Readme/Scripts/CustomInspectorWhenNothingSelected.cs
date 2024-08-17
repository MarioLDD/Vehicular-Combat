
//using UnityEditor;
//using UnityEngine;
//using ReadmeSystem;
//using ReadmeSystem.Editor;

//[InitializeOnLoad]
//public static class CustomInspectorWhenNothingSelected
//{
//    static CustomInspectorWhenNothingSelected()
//    {
//        Selection.selectionChanged += OnSelectionChanged;
//    }

//    private static void OnSelectionChanged()
//    {
//        if (Selection.activeObject == null)
//        {
//            ShowCustomInspector();
//        }
//    }

//    private static void ShowCustomInspector()
//    {
//        EditorApplication.delayCall += () =>
//        {
//            if (Selection.activeObject == null)
//            {
//                var myScriptableObject = ReadmeEditor.GetReadmeRoot();

//               //var myScriptableObject = Resources.Load<Readme>("Readme");
//                if (myScriptableObject != null)
//                {
//                    Selection.activeObject = myScriptableObject;
//                }
//                else
//                {
//                    Debug.LogWarning("MyScriptableObject not found in Resources folder.");
//                }
//            }
//        };
//    }
//}
