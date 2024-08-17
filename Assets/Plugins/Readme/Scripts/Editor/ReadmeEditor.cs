using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace ReadmeSystem.Editor
{


    [CustomEditor(typeof(Readme))]
    [InitializeOnLoad]
    public class ReadmeEditor : UnityEditor.Editor
    {

        Readme readme { get { return (Readme)target; } }
        string nextReadmeTitle
        {
            get
            {
                if (readme.nextReadme == null)
                    return "Next";


                return readme.nextReadme.name;

            }
        }

        string prevReadmeTitle
        {
            get
            {
                if (readme.prevReadme == null)
                    return "Prev";


                return readme.prevReadme.name;
            }
        }



        static bool showInEditMode = false;

        static string kShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";

        static float kSpace = 16f;

        static ReadmeEditor()
        {
            EditorApplication.delayCall += SelectReadmeAutomatically;
        }

        static void SelectReadmeAutomatically()
        {
            if (!SessionState.GetBool(kShowedReadmeSessionStateName, false))
            {
                var readme = SelectReadme();
                if (readme)
                {
                    SessionState.SetBool(kShowedReadmeSessionStateName, true);
                }
            }
        }


        [MenuItem("Tools/README")]
        static Readme SelectReadme()
        {
            showInEditMode = false;

            Readme result = GetReadmeRoot();


            if (result != null)
            {
                Selection.objects = new UnityEngine.Object[] { result };

            }
            else
            {
                Debug.LogWarning("Couldn't find a readme");
            }

            return result;

        }


        protected override void OnHeaderGUI()
        {
            var readme = (Readme)target;

            if (showInEditMode)
            {
                base.OnHeaderGUI();
                return;
            }

            DrawHeaderGUI(readme);
        }

        public override void OnInspectorGUI()
        {
            var readme = (Readme)target;

            if (showInEditMode)
            {
                base.OnInspectorGUI();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                showInEditMode = EditorGUILayout.Toggle("Show in Edit Mode", showInEditMode);

                EditorGUI.BeginChangeCheck();
                readme.isRoot = EditorGUILayout.Toggle("Set as Root", readme.isRoot);
                if (EditorGUI.EndChangeCheck())
                {
                    if (readme.isRoot)
                    {
                        Debug.Log("ResetAll");
                        //Ensures that there is only one readme as root.
                        ResetAllRootReadme();
                        readme.isRoot = true;
                    }
                }
                GUILayout.EndVertical();

                if (GUILayout.Button("Clean Seaction", GUILayout.MaxHeight(40)))
                {
                    CleanSection(readme);
                }

                GUILayout.EndHorizontal();
                if (GUILayout.Button("Update Sections Label"))
                {
                    ResetSectionsLabel(readme);
                }

                EditorGUILayout.EndVertical();

                //ImportOptions
                return;
            }


            DrawInspectorGUI(readme);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginHorizontal();

            showInEditMode = EditorGUILayout.Toggle("Show in Edit Mode", showInEditMode);
            bool root = IsDoneChecker(readme);
            if (GUILayout.Button("Generate next", EditorStyles.toolbarButton))
            {
                NewRootReadme(readme, root);
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(EditorStyles.toolbar);


            GUI.enabled = readme.prevReadme != null;
            if (GUILayout.Button(prevReadmeTitle, EditorStyles.toolbarButton))
            {
                Selection.objects = new UnityEngine.Object[] { readme.prevReadme };
            }

            GUI.enabled = readme.nextReadme != null;
            if (GUILayout.Button(nextReadmeTitle, EditorStyles.toolbarButton))
            {
                Selection.objects = new UnityEngine.Object[] { readme.nextReadme };

            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();
        }

        public static void DrawHeaderGUI(Readme readme)
        {
            if (readme == null)
                return;

            var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

            GUILayout.BeginHorizontal("In BigTitle");
            {

                GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
                if (!string.IsNullOrEmpty(readme.title))
                {
                    GUILayout.Label(readme.title, ReadmeEditorStyles.TitleStyle, GUILayout.ExpandHeight(true));
                }
                else
                {
                    GUILayout.Label(readme.fileName, ReadmeEditorStyles.TitleStyle, GUILayout.ExpandHeight(true));
                }

            }
            GUILayout.EndHorizontal();
        }
        public static void DrawInspectorGUI(Readme readme)
        {
            if (readme == null)
                return;
            if (readme.sections == null)
                return;

            foreach (var section in readme.sections)
            {
                if (!string.IsNullOrEmpty(section.heading))
                {
                    section.name =section.heading;

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(section.heading, ReadmeEditorStyles.HeadingStyle);

                    // Agregar el checkbox
                    EditorGUI.BeginChangeCheck();
                    section.isDone = GUILayout.Toggle(section.isDone, "Done");
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (section.isDone)
                        {
                            Debug.Log($"Section: {section.heading} Is Done");
                            section.isDone = true;
                            EditorUtility.SetDirty(readme);
                        }
                        else
                        {
                            Debug.Log($"Section: {section.heading} Is Undone");
                            section.isDone = false;
                            EditorUtility.SetDirty(readme);
                        }
                    }

                    GUILayout.EndHorizontal();
                    if (!section.isDone)
                    {
                        //Add Horizontal Bar
                        if (section.heading != "") { EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); }
                    }
                }
                if (!section.isDone)
                {
                    if (!string.IsNullOrEmpty(section.text))
                    {
                        if (string.IsNullOrEmpty(section.name))
                            section.name = "Text: " + section.text;

                        GUILayout.Label(section.text, ReadmeEditorStyles.BodyStyle);
                    }
                    if (!string.IsNullOrEmpty(section.linkText))
                    {

                        if (string.IsNullOrEmpty(section.name))
                            section.name = "Link: " + section.text;

                        if (ReadmeEditorStyles.LinkLabel(new GUIContent(section.linkText)))
                        {
                            Application.OpenURL(section.url);
                        }
                    }

                    GUILayout.Space(kSpace);
                    GUILayout.Space(kSpace);
                }
                GUILayout.Space(kSpace);
                //if (section.name.Length > 20)
                //{
                //    section.name = section.name.Remove(17);
                //    section.name += "...";
                //}

            }
        }
        public static void DrawImport(Readme readme)
        {

        }
        private void ResetSectionsLabel(Readme readme)
        {

            foreach (var section in readme.sections)
            {
                section.name = "";

                if (!string.IsNullOrEmpty(section.heading))
                {
                    section.name =section.heading;

                }
                if (!string.IsNullOrEmpty(section.text))
                {
                    if (string.IsNullOrEmpty(section.name))
                        section.name = "Text: " + section.text;

                }
                if (!string.IsNullOrEmpty(section.linkText))
                {

                    if (string.IsNullOrEmpty(section.name))
                        section.name = "Link: " + section.text;


                }

                //if (section.name.Length > 20)
                //{
                //    section.name = section.name.Remove(17);
                //    section.name += "...";
                //}

            }

        }

        static List<Readme> GetAllRootReadme()
        {
            var ids = AssetDatabase.FindAssets("Readme t:Readme");
            List<Readme> results = new List<Readme>();

            foreach (string guid in ids)
            {
                var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid));

                Readme readme = (Readme)readmeObject;
                if (readme.isRoot)
                {
                    results.Add(readme);

                }
            }

            return results;
        }
        public static Readme GetReadmeRoot()
        {
            Readme result = GetAllRootReadme().FirstOrDefault();
            //Ensures that there is only one readme as root.
            if (result != null)
            {
                ResetAllRootReadme();
                result.isRoot = true;
            }
            return result;

        }
        static void ResetAllRootReadme()
        {
            foreach (Readme readme in GetAllRootReadme())
            {
                readme.isRoot = false;
            }
        }

        public static bool IsDoneChecker(Readme readme)
        {
            if (readme == null)
                return false;

            foreach (var section in readme.sections)
            {
                if (!section.isDone)
                    return false;
            }
            return true;
        }

        public static void CleanSection(Readme readme)
        {
            if (readme == null || readme.sections == null)
                return;

            // Utilizar LINQ para filtrar las secciones que no están "done"
            readme.sections = readme.sections.Where(section => !section.isDone).ToArray();

            // Marcar el objeto como sucio para que Unity sepa que necesita ser guardado
            EditorUtility.SetDirty(readme);
        }

        public static void NewRootReadme(Readme readme, bool root)
        {
            string name = NameFormat(readme.name);

            string path = $"Assets/ReadmeFolder/{name}";
            if (!Directory.Exists("Assets/ReadmeFolder"))
            {
                Directory.CreateDirectory("Assets/ReadmeFolder");
            }

            Readme settings = ScriptableObject.CreateInstance<Readme>();
            settings.prevReadme = readme;
            readme.nextReadme = settings;
            if (root)
            {
                readme.isRoot = false;
                settings.isRoot = true;
            }

            //int idNumber = int.Parse(readme.id);
            //idNumber++;
            //settings.id = idNumber.ToString("D2");
            settings.fileName = name;
            //string name = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(settings, path + ".asset");
            //settings.fileName = settings.name;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = settings;
            showInEditMode = true;
            //Debug.Log("Readme created at " + settings.name);


        }

        static string NameFormat(String readmeName)
        {
            string pattern = @"^(.+)\((\d{2})\)$";

            Match match = Regex.Match(readmeName, pattern);

            if (match.Success)
            {
                string name = match.Groups[1].Value;
                int number = int.Parse(match.Groups[2].Value);

                number++;

                string newNumber = number.ToString("D2");
                string result = $"{name}({newNumber})";

                return result;
            }
            else
            {
                Console.WriteLine("El string no tiene el formato esperado.");
                return readmeName + "(01)";
            }
        }

    }

}