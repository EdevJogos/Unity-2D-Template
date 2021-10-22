using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class EventsWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Tracked Events")]
    public static void ShowExample()
    {
        EventsWindow wnd = GetWindow<EventsWindow>();
        wnd.titleContent = new GUIContent("Tracked Events");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Project/Editor/EventsElements.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        CreateEventsButtons();
    }

    private void CreateEventsButtons()
    {
        ScrollView scrollView = rootVisualElement.Q<ScrollView>("events-scroll");

        foreach (var __trackedEvent in EventsDatabase.EventsList.Values)
        {
            Button __button = new Button(__trackedEvent.Invoke);
            __button.text = __trackedEvent.ID.ToString();
            scrollView.Add(__button);

            foreach (var __trackedAction in __trackedEvent.trackedActions)
            {
                Button __actionButton = new Button(__trackedAction.Invoke);
                __actionButton.text = __trackedAction.label;
                scrollView.Add(__actionButton);
            }
        }
    }
}