using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class CarouselMenu : MonoBehaviour
{
    [SerializeField] private float slideDuration = 0.4f; 
    [SerializeField] private RectTransform[] panelsSons;
    
    private float panelWidth;
    private RectTransform rect;
    private int currentIndex;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        
        // Obtenemos el ancho REAL dinámico de este contenedor
        panelWidth = rect.rect.width;
        
        // OFFSET AUTOMÁTICO
        // Recorremos cada panel y lo movemos a su posición exacta
        for (int i = 0; i < panelsSons.Length; i++)
        {
            // Forzamos el ancho exacto
            panelsSons[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panelWidth);
            // Panel 0 -> Pos X: 0
            // Panel 1 -> Pos X: Ancho * 1
            // Panel 2 -> Pos X: Ancho * 2
            // Así eliminamos cualquier hueco o superposición
            panelsSons[i].anchoredPosition = new Vector2(panelWidth * i, 0);
        }
        
        // POSICIÓN INICIAL: FUERA DE PANTALLA
        // Lo movemos a la derecha (positivo) una distancia igual al ancho.
        // Al estar la "VentanaMascara" quieta, esto lo oculta totalmente.
        rect.anchoredPosition = new Vector2(panelWidth, 0);
    }

    public void GoToNextPanel()
    {
        currentIndex++;

        // si estamos en el último panel → cargar escena
        if (currentIndex >= panelsSons.Length)   // 0=Lore, 1=Mov, 2=Adv
        {
            SceneManager.LoadScene("AtrapalosGameplay");
            return;
        }

        // Movemos el contenedor hacia la izquierda (-ancho * indice)
        StartCoroutine(SlideTo(-panelWidth * currentIndex));
    }

    public void GoToFirstPanel()
    {
        // Play button llama esto
        currentIndex = 0;
        StartCoroutine(SlideTo(0));
    }

    IEnumerator SlideTo(float targetX)
    {
        float elapsed = 0f;
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = new Vector2(targetX, 0);

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / slideDuration;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        rect.anchoredPosition = endPos;
    }
    
    public void ExitGame()
    { 
        AudioManager audio = FindFirstObjectByType<AudioManager>();
        if (audio) audio.ChangeToMenuMusic();
        SceneManager.LoadScene("Main");
        
    }
}
