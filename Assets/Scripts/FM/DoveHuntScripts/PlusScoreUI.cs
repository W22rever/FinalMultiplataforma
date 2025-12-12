using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlusScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private float fadeDuration = 0.5f;
    private float moveUpDistance = 1.5f;
    private float totalDuration = 1.2f;

    public void StartAppear()
    {
        // Fade inicial
        text.alpha = 0f;

        Sequence sequence = DOTween.Sequence();

        // Fade in
        sequence.Append(text.DOFade(1f, fadeDuration));

        // Movimiento hacia arriba
        sequence.Join(transform.DOMove(transform.position + Vector3.up * moveUpDistance, totalDuration));

        // Fade out
        sequence.Append(text.DOFade(0f, fadeDuration));

        // Auto-destruir al terminar
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
