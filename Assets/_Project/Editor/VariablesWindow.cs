using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class VariablesWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Tracked Variables")]
    public static void ShowExample()
    {
        VariablesWindow wnd = GetWindow<VariablesWindow>();
        wnd.titleContent = new GUIContent("Tracked Variables");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Project/Editor/VariablesElements.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        CreateVariablesList();

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        /*var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Project/Editor/TrackedStyles.uss");
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);*/
    }

    

    private void CreateVariablesList()
    {
        ScrollView scrollView = rootVisualElement.Q<ScrollView>("variables-scroll");

        Debug.Log(EditorVariableData.EditorDataList.Count);
        for (int __i = 0; __i < EditorVariableData.EditorDataList.Count; __i++)
        {
            EditorVariableData __editorData = EditorVariableData.EditorDataList[__i];

            VisualElement __label = new Label(__editorData.label);
            scrollView.Add(__label);

            for (int __j = 0; __j < __editorData.trackedFloats.Count; __j++)
            {
                int __index = __j;
                EditorVariable<float> __trackedFloat = __editorData.trackedFloats[__j];

                FloatField __field = new FloatField(__trackedFloat.label, 100);
                
                __field.RegisterValueChangedCallback(x =>
                {
                    __trackedFloat.variable.Value = x.newValue;
                });

                scrollView.Add(__field);
            }
        }
    }
}