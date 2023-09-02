using UnityEngine;
using UnityEngine.UI;

namespace ETemplate.UI
{
    public class JoinUI : UISelectable
    {
        public bool Ready => _empty || _readyButton.Charged;

        public ButtonHold ReadyButton => _readyButton;
        public UIButton LeftButton => _leftButton;
        public UIButton RightButton => _rightButton;

        [SerializeField] private Image _image;
        [SerializeField] private TMPro.TextMeshProUGUI _description;
        [SerializeField] private ButtonHold _readyButton;
        [SerializeField] private UIButton _leftButton, _rightButton;

        private bool _empty = true;

        public void Empty(Sprite p_sprite, string p_description)
        {
            UpdateCharacter(p_sprite);
            UpdateDescription(p_description);

            _readyButton.Enabled = false;
            _leftButton.Enabled = _rightButton.Enabled = false;
            _empty = true;
        }

        public void Initialize(Sprite p_sprite, string p_description)
        {
            UpdateUI(p_sprite, p_description);

            _readyButton.Enabled = true;
            _leftButton.Enabled = _rightButton.Enabled = true;
            _empty = false;
        }

        public void UpdateUI(Sprite p_sprite, string p_description)
        {
            UpdateCharacter(p_sprite);
            UpdateDescription(p_description);
            _readyButton.Restart();
        }

        private void UpdateCharacter(Sprite p_sprite) => _image.sprite = p_sprite;
        private void UpdateDescription(string p_text) => _description.text = p_text;
    }
}
