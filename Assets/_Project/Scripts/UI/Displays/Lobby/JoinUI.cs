using UnityEngine;
using UnityEngine.UI;

public class JoinUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMPro.TextMeshProUGUI _description;

    public void UpdateCharacter(Sprite p_sprite)
    {
        _image.sprite = p_sprite;
    }

    public void UpdateDescription(string p_text)
    {
        _description.text = p_text;
    }
}
