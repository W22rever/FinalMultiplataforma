using UnityEngine;
using UnityEngine.UI;
public class UILifePoints : MonoBehaviour
{
    [SerializeField] private Image[] hearts;

    private void OnEnable()
    {
        GameEvents.OnLifeChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        GameEvents.OnLifeChanged -= UpdateHearts;
    }

    private void UpdateHearts(int currentLife)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLife)
                hearts[i].color = Color.white;  // vida presente
            else
                hearts[i].color = Color.black;  // vida perdida
        }
    }
}
