using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameCEO : MonoBehaviour
{
    private static GameCEO Instance;

    public static event System.Action onGameStateChanged;

    public static bool Tutorial { get; private set; } = true;
    public static GameState State { get; private set; }

    #if UNITY_EDITOR
    public GameState state;
#endif

    [SerializeField] private Transform _managersHolder;

    private GUIManager _guiManager;
    private InputManager _inputManager;
    private CameraManager _cameraManager;
    private AudioManager _audioManager;
    private AgentsManager _agentsManager;
    private StageManager _stageManager;

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
        foreach (Transform __transform in _managersHolder)
        {
            if(__transform.TryGetComponent(out Manager __manager))
            {
                switch(__manager)
                {
                    case GUIManager:  _guiManager = __manager as GUIManager; break;
                    case InputManager: _inputManager = __manager as InputManager; break;
                    case CameraManager: _cameraManager = __manager as CameraManager; break;
                    case AudioManager: _audioManager = __manager as AudioManager; break;
                    case AgentsManager: _agentsManager = __manager as AgentsManager; break;
                    case StageManager: _stageManager = __manager as StageManager; break;
                }

                __manager.Initiate();
            }
        }

        _guiManager.onJoinRequested += GUIManager_onJoinRequested;
        _guiManager.onSwitchCharacterRequested += _guiManager_onSwitchCharacterRequested;

        _inputManager.onPlayerJoined += _inputManager_onPlayerJoined;
    }

    public void Initialize()
    {
        foreach (Transform __transform in _managersHolder)
        {
            if (__transform.TryGetComponent(out Manager __manager))
            {
                __manager.Initialize();
            }
        }

        _guiManager.ShowDisplay(Displays.INTRO);
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
}
