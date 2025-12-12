using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    private RectTransform rtMainMenu;
    private RectTransform rtPanelMain;
    private RectTransform rtInstruction;

    private GameObject panelOnPlay;
    private Button btnPause;
    private bool startGame;
    private Vector2 initialMenuPos;
    private Ducks[] doves;
    //private Gun _gun;
    
    [HideInInspector] public GameObject panelOnPause;
    [HideInInspector] public bool ignoreNextClick;
    
    [SerializeField] private AudioManager audioManager;
    
    [Header("Victory UI")]
    [SerializeField] private GameObject panelVictory;
    [SerializeField] private TextMeshProUGUI titleVictory;
    [SerializeField] private Button restartGame;
    [SerializeField] private Button backToMainMenu;
    
    void Awake()
    {
        rtMainMenu = GameObject.Find("MainMenu").GetComponent<RectTransform>();
        rtPanelMain = GameObject.Find("MainMenu/PanelMain").GetComponent<RectTransform>();
        rtInstruction = GameObject.Find("MainMenu/PanelInstruction").GetComponent<RectTransform>();
        
        btnPause = GameObject.Find("PauseButton").GetComponent<Button>();
        
        panelOnPlay = GameObject.Find("PanelOnPlay");
        panelOnPause = GameObject.Find("PanelOnPause");

        //_gun = GameObject.Find("Gun").GetComponent<Gun>();
        
        initialMenuPos = rtMainMenu.anchoredPosition;

        panelOnPlay.gameObject.SetActive(false);
        panelOnPause.gameObject.SetActive(false);
        panelVictory.gameObject.SetActive(false);
        
        doves = FindObjectsByType<Ducks>(FindObjectsSortMode.None);
        //Debug.Log(doves.Length);
    }
    

    private IEnumerator MoveUI(RectTransform rt, Vector2 target, float duration)
    {
        Vector2 startPosition = rt.anchoredPosition;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            rt.anchoredPosition = Vector2.Lerp(startPosition, target, t);
            yield return null;
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameSequence());
    }
    
    private IEnumerator StartGameSequence()
    {
        panelOnPlay.SetActive(true);
        
        // Obtenemos el ancho de la pantalla en base a los anclajes
        float widthScreen = rtMainMenu.rect.width;
        
        // 1. Esperas a que el panel termine de moverse
        yield return StartCoroutine(MoveUI(rtMainMenu, new Vector2(widthScreen * 1.1f, 0), 2f));
        
        
        startGame = true;
    }
    
    public bool GameStarted => startGame;

    public void ShowInstruction()
    {
        StartCoroutine(MoveUI(rtInstruction, new Vector2(0, 0), 1.6f ));
        StartCoroutine(MoveUI(rtPanelMain, new Vector2(0, 1080), 0.3f ));
    }

    public void HideInstruction()
    {
        StartCoroutine(MoveUI(rtInstruction, new Vector2(0, -1080), 0.3f ));
        StartCoroutine(MoveUI(rtPanelMain, new Vector2(0, 0), 1.6f ));
    }

    public void ExitGame()
    { 
        // Si estamos en el editor de Unity, paramos el juego como antes
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estamos en el móvil (build real), cerramos la aplicación
        Application.Quit();
#endif
    }

    public void PauseGame()
    {
        btnPause.interactable = false;
        Time.timeScale = 0;
        panelOnPause.SetActive(true);
    }

    public void ResumeGame()
    {
        panelOnPause.SetActive(false);
        btnPause.interactable = true;
        
        Time.timeScale = 1;

        StartCoroutine(IgnoreInputOneFrame());
    }
    
    private IEnumerator IgnoreInputOneFrame()
    {
        ignoreNextClick = true;
        yield return null; // ignorar el frame del click usado en el botón Resume
        ignoreNextClick = false;
    }
    
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        startGame = false;
        ignoreNextClick = false;

        panelOnPlay.SetActive(false);
        panelOnPause.SetActive(false);
        
        //audioManager.ChangeToMenuMusic();
        StartCoroutine(ReturnMenuSequence());
    }
    
    
    private IEnumerator ReturnMenuSequence()
    {
        // Mover menú al centro
        yield return StartCoroutine(MoveUI(rtMainMenu, initialMenuPos, 1.2f));

        // Reiniciar gameplay después que el menú ya apareció visualmente
        ResetScenario();
    }
    
    
    public void ResetScenario()
    {
        // Activamos la interacción con el botón de pausa
        btnPause.interactable = true;
        
        // Reiniciar Score global llamando al reset del Bullet
        Bullet.ResetScore?.Invoke();

        // Resetear arma
        Gun gun = FindFirstObjectByType<Gun>(); 
        if (gun) gun.ResetGun();
        
        PlusScoreUI plusScore = FindFirstObjectByType<PlusScoreUI>();
        if(plusScore) plusScore.gameObject.SetActive(false);
        if(panelVictory.gameObject.activeSelf) panelVictory.gameObject.SetActive(false);
        
        foreach (var dove in doves)
        {
            if(!dove.gameObject.activeInHierarchy) dove.gameObject.SetActive(true);
            //Debug.Log(dove.gameObject.name + $"Is active: {(dove.gameObject.activeSelf ? "yes" : "no" )} ");
            dove.ResetDuck();
        }
    }

    public void RestartLevel()
    {
        ResetScenario();
        
        // Desactivar panel de Pausa
        panelOnPause.SetActive(false);
        if(panelVictory.gameObject.activeSelf) panelVictory.gameObject.SetActive(false);
       
        // Activamos el gameplay
        startGame = true;
        
        Time.timeScale = 1;
    }

    public void Victory()
    {
         StartCoroutine(PanelVictorySequency());
    }

    private IEnumerator PanelVictorySequency()
    {
        yield return new WaitForSeconds(0.2f);
        panelVictory.SetActive(true);
    }
}
