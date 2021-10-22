using System.Collections;
using UnityEngine;

public class Display : MonoBehaviour
{
    public static System.Action<Displays, int> onActionRequested;
    public static System.Action<Displays, int, object> onDataActionRequested;

    public DisplayTypes type;
    public Displays ID;
    public UnityEngine.UI.Selectable startSelected;

    public virtual void Initiate()
    {

    }

    public virtual void Initialize()
    {

    }

    public virtual void Show(bool p_show, System.Action p_callback, float p_ratio)
    {
        if (p_show)
        {
            if (startSelected != null)
            {
                startSelected.Select();
            }
            else
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }

            GetComponent<Animator>().SetTrigger("In");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Out");
        }

        if (p_callback != null) StartCoroutine(RoutineShow(p_show, p_callback, p_ratio));
    }

    private IEnumerator RoutineShow(bool p_show, System.Action p_callback, float p_ratio)
    {
        string __clipName = p_show ? "In" : "Out";

        float __delay = GetClipLength(__clipName) * GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).speed * p_ratio;
        
        yield return new WaitForSecondsRealtime(__delay);

        p_callback?.Invoke();
    }

    private float GetClipLength(string name)
    {
        AnimationClip[] clips = GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach (var item in clips)
        {
            if (item.name == name)
            {
                return item.length;
            }
        }

        return 0;
    }

    public virtual void UpdateDisplay(int p_operation, bool p_value)
    {

    }

    public virtual void UpdateDisplay(int p_operation, float p_value, float p_data)
    {

    }

    public virtual void UpdateDisplay(int p_operation, float[] p_data)
    {

    }

    public virtual void UpdateDisplay(int p_operation, object p_data)
    {

    }

    public virtual object GetData(int p_data)
    {
        return null;
    }

    public virtual void RequestAction(int p_action)
    {
        onActionRequested?.Invoke(ID, p_action);
    }

    public virtual void RequestAction(int p_action, object p_data)
    {
        onDataActionRequested?.Invoke(ID, p_action, p_data);
    }
}
