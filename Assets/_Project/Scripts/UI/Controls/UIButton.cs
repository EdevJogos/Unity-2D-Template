using UnityEngine;
using TMPro;

namespace ETemplate.UI
{
    public class UIButton : UISelectable
    {
        public bool Enabled
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }
    }
}
