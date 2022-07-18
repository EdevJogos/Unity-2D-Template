using UnityEngine;

public class DatabasesHandler : MonoBehaviour
{
    private void Awake()
    {
        foreach (Transform __transform in transform)
        {
            IDatabase __database = __transform.GetComponent<IDatabase>();

            if (__database == null)
                return;

            __database.Initiate();
        }
    }
}
