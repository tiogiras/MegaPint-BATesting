using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MegaPint
{

/// <summary> Handles winning and losing in the task test procedure </summary>
[AddComponentMenu("")]
internal class SceneManagerTestProcedure : MonoBehaviour
{
    public static Action onWin;

    [SerializeField] private List <FadeGraphic> _loseGraphics;
    [SerializeField] private GameObject _winGraphic;
    [SerializeField] private GameObject _hintGraphic;
    [SerializeField] private Button _button;
    [SerializeField] private Image _background;

    private readonly List <FadeGraphic> _loseGraphicsPool = new();

    private bool _spaceIsPressed;
    private int _tryCount;

    #region Unity Event Functions

    private void Update()
    {
        if (Input.GetKeyDown("space"))
            _spaceIsPressed = true;

        if (Input.GetKeyUp("space"))
            _spaceIsPressed = false;
    }

    #endregion

    #region Public Methods

    /// <summary> Try to win the task </summary>
    public void TryWin()
    {
        _tryCount++;

        if (_spaceIsPressed)
            Win();
        else
            Lose();
    }

    #endregion

    #region Private Methods

    /// <summary> Get a fade graphic from the pool </summary>
    /// <returns> Found fade graphic </returns>
    private FadeGraphic GetFromPool()
    {
        if (_loseGraphicsPool.Count == 0)
        {
            foreach (FadeGraphic loseGraphic in _loseGraphics)
                _loseGraphicsPool.Add(loseGraphic);
        }

        FadeGraphic graphic = _loseGraphicsPool[Random.Range(0, _loseGraphicsPool.Count)];
        _loseGraphicsPool.Remove(graphic);

        return graphic;
    }

    /// <summary> Lose the task </summary>
    private void Lose()
    {
        if (_tryCount == 5)
            _hintGraphic.SetActive(true);

        GetFromPool().FadeIn();
    }

    /// <summary> Win the task </summary>
    private void Win()
    {
        _winGraphic.SetActive(true);
        _button.interactable = false;
        _background.color = new Color(0.09f, 0.51f, 0.09f);

        onWin?.Invoke();
    }

    #endregion
}

}
