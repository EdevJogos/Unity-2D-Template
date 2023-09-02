using UnityEngine;
using UnityEngine.InputSystem;

public partial class GameCEO
{
    private void DisplayIntro()
    {
        _guiManager.ShowDisplay(Displays.INTRO);
    }

    private void DisplayCredits()
    {
        _guiManager.ShowDisplay(Displays.CREDITS);
    }

    private void DisplaySettings()
    {
        _guiManager.ShowDisplay(Displays.SETTINGS);
    }
}
