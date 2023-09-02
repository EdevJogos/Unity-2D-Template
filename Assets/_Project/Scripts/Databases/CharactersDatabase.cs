using UnityEngine;

public class CharactersDatabase : MonoBehaviour, IDatabase
{
    [System.Serializable]
    public struct CharacterData
    {
        public string name;
        public Sprite sprite;
    }

    private static CharactersDatabase s_Instance;

    [SerializeField] private CharacterData[] _characters;

    public void Initiate()
    {
        s_Instance = this;
    }

    public static CharacterData GetCharacterData(int p_index)
    {
        return s_Instance._characters[p_index];
    }
}