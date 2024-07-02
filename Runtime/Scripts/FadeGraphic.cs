// TODO commenting

using UnityEngine;
using UnityEngine.UI;

namespace MegaPint
{

[AddComponentMenu("")]
internal class FadeGraphic : MonoBehaviour
{
    [SerializeField] private Graphic _graphic;
    [SerializeField] private float _fadeSpeed = 1f;

    #region Unity Event Functions

    private void Awake()
    {
        SetAlpha(0);
    }

    private void Update()
    {
        if (_graphic.color.a <= 0)
            return;

        var newAlpha = _graphic.color.a - _fadeSpeed * Time.deltaTime;
        newAlpha = Mathf.Clamp01(newAlpha);

        SetAlpha(newAlpha);
    }

    #endregion

    #region Public Methods

    public void FadeIn()
    {
        SetAlpha(1f);
    }

    #endregion

    #region Private Methods

    private void SetAlpha(float alpha)
    {
        Color color = _graphic.color;
        color.a = alpha;
        _graphic.color = color;
    }

    #endregion
}

}
