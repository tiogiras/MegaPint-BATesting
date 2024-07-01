using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MegaPint.com.tiogiras.megapint_batesting.Runtime.Scripts
{
    public class TestProcedure : MonoBehaviour
    {
        [SerializeField] private List<Graphic> _loseGraphics;
        
        public void TryWin()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Lose();
            else
                Win();
        }

        private void Lose()
        {
            
        }

        private void Win()
        {
            
        }
    }
}
