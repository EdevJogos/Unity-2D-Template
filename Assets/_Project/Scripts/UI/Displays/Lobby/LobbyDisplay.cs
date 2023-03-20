using System;
using UnityEngine;
using static CharactersDatabase;

public class LobbyDisplay : Display
{
    public const int JOIN = 1;

    public const int UPDATE_JOINED_STATE = 10;
    public const int SWITCH_CHARACTER = 11;
    public const int UPDATE_CHARACTER = 12;

    [SerializeField] private JoinUI[] _joinUIs;

    public override void Show(bool p_show, Action p_onCompleted, float p_ratio)
    {
        if(p_show)
        {
            InputManager.Instance.GeneralIO.General.Join.performed += Join_performed;
        }
        else
        {
            InputManager.Instance.GeneralIO.General.Join.performed -= Join_performed;
        }

        base.Show(p_show, p_onCompleted, p_ratio);
    }

    public override void UpdateDisplay(int p_operation, float p_value, float p_data)
    {
        base.UpdateDisplay(p_operation, p_value, p_data);

        switch (p_operation)
        {
            case UPDATE_JOINED_STATE:
                UpdateJoinedState((int)p_value);
                break;
        }
    }

    public override void UpdateDisplay(int p_operation, object p_data)
    {
        base.UpdateDisplay(p_operation, p_data);

        switch (p_operation)
        {
            case UPDATE_CHARACTER:
                object[] __data = p_data as object[];
                int __id = (int)__data[0];
                CharacterData __characterData = (CharacterData)__data[1];
                UpdateCharacter(__id, __characterData);
                break;
        }
    }

    protected override void HandleHorizontalMovementDelayed(int p_id, int p_direction)
    {
        base.HandleHorizontalMovementDelayed(p_id, p_direction);
        RequestAction(SWITCH_CHARACTER, new object[2] { p_id, p_direction });
    }

    private void UpdateJoinedState(int p_id)
    {
        StartInputListener(p_id);
    }

    private void UpdateCharacter(int p_id, CharacterData p_characterData)
    {
        _joinUIs[p_id].UpdateCharacter(p_characterData.sprite);
        _joinUIs[p_id].UpdateDescription(p_characterData.name);
    }

    private void Join_performed(UnityEngine.InputSystem.InputAction.CallbackContext p_context)
    {
        RequestAction(JOIN);
    }
}