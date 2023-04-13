using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static CharactersDatabase;

public class LobbyDisplay : Display
{
    public const int JOIN = 1;

    public const int SETUP = 10;
    public const int UPDATE_JOINED_STATE = 11;
    public const int SWITCH_CHARACTER = 12;
    public const int UPDATE_CHARACTER = 13;
    public const int ALL_PLAYERS_READY = 14;
    public const int UPDATE_COUNTDOWN = 15;

    [SerializeField] private string _emptyDescription;
    [SerializeField] private Sprite _spriteEmpty;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private Transform _body;
    [SerializeField] private JoinUI _joinUIPrefab;

    private List<JoinUI> _joinUIs = new();

    public override void Initiate()
    {
        base.Initiate();
        for (int __i = 0; __i < GameConfig.Game.MAX_PLAYERS; __i++)
        {
            CreateJoinUI(__i);
        }
    }

    public override void Show(bool p_show, Action p_onCompleted, float p_ratio)
    {
        if(p_show)
        {
            _startSelected = _joinUIs[0];
            _countdownText.enabled = false;

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
            case UPDATE_COUNTDOWN:
                UpdateCountDown((int)p_value);
                break;
        }
    }

    public override void UpdateDisplay(int p_operation, object p_data)
    {
        base.UpdateDisplay(p_operation, p_data);

        switch (p_operation)
        {
            case UPDATE_CHARACTER:
                object[] __updateData = p_data as object[];
                int __id = (int)__updateData[0];
                CharacterData __characterData = (CharacterData)__updateData[1];
                UpdateCharacter(__id, __characterData);
                break;
            case SETUP:
                List<int> __setupData = p_data as List<int>;
                Setup(__setupData);
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

    private void Setup(List<int> p_charIDs)
    {
        for (int __i = 0; __i < p_charIDs.Count; __i++)
        {
            JoinUIInitialize(__i, GetCharacter(p_charIDs[__i]));
        }
    }

    private void CreateJoinUI(int p_id)
    {
        JoinUI __joinUi = Instantiate(_joinUIPrefab, _body);

        __joinUi.Empty(_spriteEmpty, _emptyDescription);
        __joinUi.LeftButton.onPointerUp.AddListener(() => { HandleHorizontalMovementDelayed(p_id, -1); });
        __joinUi.RightButton.onPointerUp.AddListener(() => { HandleHorizontalMovementDelayed(p_id, 1); });
        __joinUi.ReadyButton.onCharged.AddListener(() => { CheckAllPlayersReady(p_id); });
        _joinUIs.Add(__joinUi);
    }

    private void CheckAllPlayersReady(int p_id)
    {
        bool __allReady = _joinUIs.TrueForAll(jui => jui.Ready);

        if(__allReady)
        {
            RequestAction(ALL_PLAYERS_READY);
        }
    }

    private void JoinUIInitialize(int p_id, CharacterData p_characterData)
    {
        _joinUIs[p_id].Initialize(p_characterData.sprite, p_characterData.name);
    }
    private void UpdateCharacter(int p_id, CharacterData p_characterData)
    {
        _joinUIs[p_id].UpdateUI(p_characterData.sprite, p_characterData.name);
    }

    private void UpdateCountDown(int p_countdown)
    {
        _countdownText.enabled = true;
        _countdownText.text = p_countdown != 0 ? p_countdown + "" : "Start!";
    }

    private void Join_performed(UnityEngine.InputSystem.InputAction.CallbackContext p_context)
    {
        RequestAction(JOIN);
    }
}