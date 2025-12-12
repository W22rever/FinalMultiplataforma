using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // IMPORTANTE: Aseguramos que el tiempo corra normal
        // (Por si vienes de una pausa o Game Over donde el tiempo es 0)
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(sceneName);
    }

    public void QuitAplication()
    {
#if UNITY_EDITOR
        // Si estamos probando en el Editor de Unity
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // Si es una Build (Móvil, PC, Consola, etc.)
    Application.Quit();
#endif
    }
}