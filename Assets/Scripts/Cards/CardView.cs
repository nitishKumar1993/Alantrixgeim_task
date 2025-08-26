using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MatchingGame
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Transform face;
        [SerializeField] private Transform back;
        [SerializeField] private Image faceRenderer;

        public bool IsFaceUp { get; private set; }

        public void SetFace(Sprite sprite) => faceRenderer.sprite = sprite;

        Coroutine currentFlipCR;

        /// <summary>
        /// Flips the card on selection
        /// </summary>
        public void Flip(bool faceUp, float duration)
        {
            if (this.gameObject.activeInHierarchy)
            {
                currentFlipCR = StartCoroutine(Flip_CR(faceUp, duration));
            }
        }

        public void ShowFace()
        {
            Flip(true, 0.5f);
        }

        public void HideFace()
        {
            Flip(false, 0.5f);
        }

        /// <summary>
        /// Animates the fliping card
        /// Flips the visible face halfway to hide and then flip then
        /// flip the next face from halfway to full to show
        /// </summary>
        private IEnumerator Flip_CR(bool faceUp, float duration)
        {
            if (IsFaceUp == faceUp) yield break;
            IsFaceUp = faceUp;

            Transform hide = faceUp ? back : face;
            Transform show = faceUp ? face : back;
            float half = duration * 0.5f;

            hide.localScale = new Vector3(Mathf.Max(0, hide.localScale.x), 1f, 1f);
            show.localScale = new Vector3(0, 1f, 1f);

            float t = 0f;
            while (t < half)
            {
                t += Time.deltaTime;
                float s = 1f - Mathf.Clamp01(t / half);
                hide.localScale = new Vector3(Mathf.Max(0, s), 1f, 1f);
                yield return null;
            }
            hide.localScale = new Vector3(0, 1f, 1f);

            t = 0f;
            while (t < half)
            {
                t += Time.deltaTime;
                float s = Mathf.Clamp01(t / half);
                show.localScale = new Vector3(Mathf.Max(0, s), 1f, 1f);
                yield return null;
            }
            show.localScale = Vector3.one;
        }

        private void OnDisable()
        {
            if (currentFlipCR != null) StopCoroutine(currentFlipCR);
        }
    }
}