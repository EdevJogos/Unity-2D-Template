using UnityEngine;
using static CharactersDatabase;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bodyRenderer;

    public virtual void Setup(CharacterData p_data)
    {
        _bodyRenderer.sprite = p_data.sprite;
    }
}
