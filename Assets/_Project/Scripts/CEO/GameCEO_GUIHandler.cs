using UnityEngine;
using UnityEngine.InputSystem;
using static CharactersDatabase;

public partial class GameCEO
{
    private void GUIManager_onJoinRequested(object p_data)
    {
        _inputManager.Join(Gamepad.current);
    }
    private void _guiManager_onSwitchCharacterRequested(object p_data)
    {
        object[] __data = p_data as object[];
        int __id = (int)__data[0];
        int __direction = (int)__data[1];
        int __index = _agentsManager.characterIndex[__id] = HelpExtensions.ClampCircle(_agentsManager.characterIndex[__id] + __direction, 0, 2);
        CharacterData __character = GetCharacter(__index);

        _guiManager.UpdateDisplay(Displays.LOBBY, LobbyDisplay.UPDATE_CHARACTER, new object[2] { __id, __character });
    }
}
