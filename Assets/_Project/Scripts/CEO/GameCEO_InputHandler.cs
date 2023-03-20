using UnityEngine.InputSystem;
using static CharactersDatabase;

public partial class GameCEO
{
    private void _inputManager_onPlayerJoined(int p_id)
    {
        _guiManager.UpdateDisplay(Displays.LOBBY, LobbyDisplay.UPDATE_JOINED_STATE, p_id);

        CharacterData __character = GetCharacter(_agentsManager.characterIndex[p_id]);
        _guiManager.UpdateDisplay(Displays.LOBBY, LobbyDisplay.UPDATE_CHARACTER, new object[2] { p_id, __character });
    }
}
