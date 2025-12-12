using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private RectTransform panel;

    void Awake()
    {
        panel = GetComponent<RectTransform>();
        Refresh();
    }

    void Update()
    {
        // Solo refrescamos si cambia la orientación (útil para pruebas)
        Refresh();
    }

    void Refresh()
    {
        Rect safeArea = Screen.safeArea;

        // Convertimos los píxeles del área segura a coordenadas normalizadas (0 a 1) para los Anchors
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // Aplicamos al panel
        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;
        
        // Reseteamos offsets para que encaje perfecto
        panel.offsetMin = Vector2.zero; 
        panel.offsetMax = Vector2.zero;
    }
}
