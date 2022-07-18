using UnityEngine;

public class PrefabsDatabase : MonoBehaviour, IDatabase
{
    [System.Serializable]
    public struct PrefabData
    {
        public Prefabs ID;
        public GameObject[] prefabs;
    }

    public static PrefabsDatabase Instance;

    public PrefabData[] prefabs;

    public void Initiate()
    {
        Instance = this;
    }

    public static GameObject InstantiatePrefab(Prefabs p_id, int p_index, Vector2 p_position)
    {
        GameObject __particlePrefab = GetParticle(p_id, p_index);

        return Instantiate(__particlePrefab, p_position, __particlePrefab.transform.rotation);
    }

    public static T InstantiatePrefab<T>(Prefabs p_id, int p_index, Vector2 p_position, Quaternion p_rotation)
    {
        GameObject __particlePrefab = GetParticle(p_id, p_index);

        return Instantiate(__particlePrefab, p_position, p_rotation).GetComponent<T>();
    }

    public static GameObject InstantiatePrefab(Prefabs p_id, int p_index, Transform p_parent)
    {
        return Instantiate(GetParticle(p_id, p_index), p_parent);
    }

    private static GameObject GetParticle(Prefabs p_id, int p_index)
    {
        for (int __i = 0; __i < Instance.prefabs.Length; __i++)
        {
            if (Instance.prefabs[__i].ID == p_id)
            {
                return Instance.prefabs[__i].prefabs[p_index];
            }
        }

        return null;
    }
}
