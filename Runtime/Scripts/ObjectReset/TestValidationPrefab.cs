using UnityEngine;

namespace MegaPint.ObjectReset
{

// TODO remove from components menu
internal class TestValidationPrefab : ResetObjectLogic
{
    #region Public Methods

    public override void ResetLogic()
    {
        Transform myTransform = transform;
        myTransform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

        myTransform.rotation = Quaternion.Euler(
            Random.Range(-180, 180),
            Random.Range(-180, 180),
            Random.Range(-180, 180));

        gameObject.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("UI");
    }

    #endregion
}

}
