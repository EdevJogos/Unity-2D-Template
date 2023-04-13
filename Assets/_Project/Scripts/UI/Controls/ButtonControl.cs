using UnityEngine;
using TMPro;

public class ButtonControl : UIOption
{
    public bool Enabled
    { 
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }

    [SerializeField] private TextMeshProUGUI buttonText;
}
