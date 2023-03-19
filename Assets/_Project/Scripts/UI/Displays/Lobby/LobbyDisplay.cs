using UnityEngine;

public class LobbyDisplay : Display
{
    public const int JOIN = 1;

    [SerializeField] private JoinUI[] _joinUIs;

    protected override void UI_onConfirmRequested(int p_id)
    {
        base.UI_onConfirmRequested(p_id);
        RequestAction(JOIN);
    }
}