using System.Collections;
using UnityEngine;

public class MonoAssistant : MonoBehaviour
{
    private static MonoAssistant s_Instance;
    public void Initiate()
    {
        s_Instance = this;
    }

    public static void ExecuteCoroutine(IEnumerator p_enumerator)
    {
        s_Instance.StartCoroutine(p_enumerator);
    }

    public static void KillCoroutine(IEnumerator p_enumerator)
    {
        s_Instance.StopCoroutine(p_enumerator);
    }

    public static void RestartCoroutine(ref IEnumerator p_enumerator)
    {
        if (p_enumerator != null)
            s_Instance.StopCoroutine(p_enumerator);

        s_Instance.StartCoroutine(p_enumerator);
    }
}
