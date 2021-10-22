using UnityEngine;

public class DestroyOnGameOver : MonoBehaviour
{
    public bool destroyImmediately = true;

    private void Awake()
    {
        GameCEO.onGameStateChanged += GameCEO_onGameStateChanged;
    }

    private void OnDestroy()
    {
        GameCEO.onGameStateChanged -= GameCEO_onGameStateChanged;
    }

    private void GameCEO_onGameStateChanged()
    {
        if(GameCEO.State == GameState.GAME_OVER)
        {
            if(destroyImmediately)
            {
                Destroy(gameObject);
            }
            else
            {
                GetComponent<IDestructable>().RequestDestroy();
            }
        }
    }
}