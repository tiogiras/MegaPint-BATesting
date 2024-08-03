using System.Collections;
using TMPro;
using UnityEngine;

namespace MegaPint.RepairScene
{

/// <summary> Used in the repair a scene 1 to 3 task </summary>
[AddComponentMenu("")]
internal class EndMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;

    [Space]
    [SerializeField] private AnimationCurve _sizeOverLife;

    [SerializeField] private float _fadeSpeed = 1f;

    private float _fadeProgress;

    #region Unity Event Functions

    private void Awake()
    {
        _text.fontSize = 0;
    }

    #endregion

    #region Public Methods

    /// <summary> Show the message </summary>
    /// <param name="success"> If the validation was successful </param>
    public void ShowMessage(bool success)
    {
        _text.text = success ? "Success" : "Try Again";
        _text.color = success ? _successColor : _failColor;

        StopAllCoroutines();
        StartCoroutine(ShowMessageIE());
    }

    #endregion

    #region Private Methods

    /// <summary> Fade in the message </summary>
    /// <returns> Coroutine </returns>
    private IEnumerator ShowMessageIE()
    {
        _fadeProgress = 0;

        while (_fadeProgress < 1)
        {
            _text.fontSize = _sizeOverLife.Evaluate(_fadeProgress);
            _fadeProgress += Time.deltaTime * _fadeSpeed;

            yield return null;
        }

        _text.fontSize = _sizeOverLife.Evaluate(1);
    }

    #endregion
}

}
