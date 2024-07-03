// TODO commenting

using System.Collections.Generic;
using MegaPint.Editor.Scripts.Windows.TaskManagerContent.Data;
using UnityEngine;
using UnityEngine.UI;

namespace MegaPint
{

[AddComponentMenu("")]
public class TestProcedure : MonoBehaviour
{
    [SerializeField] private List <FadeGraphic> _loseGraphics;
    [SerializeField] private GameObject _winGraphic;
    [SerializeField] private GameObject _hintGraphic;
    [SerializeField] private Button _button;
    [SerializeField] private Image _background;
    [SerializeField] private Goal _goal;

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

    private void Lose()
    {
        if (_tryCount == 5)
            _hintGraphic.SetActive(true);
        
        GetFromPool().FadeIn();
    }

    private void Win()
    {
        _winGraphic.SetActive(true);
        _button.interactable = false;
        _background.color = new Color(0.09f, 0.51f, 0.09f);

        Goal.MarkGoalAsDone(_goal);
    }

    #endregion
}

}
