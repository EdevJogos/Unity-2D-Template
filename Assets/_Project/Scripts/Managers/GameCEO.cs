using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCEO : MonoBehaviour
{
    private static GameCEO Instance;

    public static event System.Action onGameStateChanged;

    public static bool Tutorial { get; private set; } = true;
    public static GameState State { get; private set; }

    #if UNITY_EDITOR
    public GameState state;
    #endif

    public GUIManager guiManager;
    public InputManager inputManager;
    public CameraManager cameraManager;
    public AudioManager audioManager;
    public AgentsManager agentsManager;
    public StageManager stageManager;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Initiate();
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initiate()
    {
        cameraManager.Initiate();
        audioManager.Initate();
        guiManager.Initiate();
    }

    public void Initialize()
    {
        audioManager.Initialize();
        guiManager.Initialize();
    }

    //-----------------CEO------------------

    private void ChangeGameState(GameState p_state)
    {
        #if UNITY_EDITOR
            state = p_state;
        #endif

        State = p_state;
        onGameStateChanged?.Invoke();
    }

    private IEnumerator RoutineLoadScene(int p_scene, float p_delay = 0f)
    {
        if (p_delay > 0) yield return new WaitForSeconds(p_delay);

        var sceneLoader = SceneManager.LoadSceneAsync(p_scene);

        while (sceneLoader.progress <= 1)
        {
            yield return null;
        }
    }

    //-----------------INPUT MANAGER------------------

    //-----------------GUI MANAGER------------------

    //-----------------SCORE MANAGER----------------

    //-----------------STAGE MANAGER----------------

    //-----------------AGENTS MANAGER----------------
}
