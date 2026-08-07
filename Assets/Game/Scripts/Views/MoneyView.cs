using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _currentMoneyText;

        [SerializeField]
        private Image _coinIcon;
        
        [SerializeField] 
        private float _duration = 0.5f;
        
        public float Duration => _duration;
        
        public Vector2 IncomeCoinPosition => this._coinIcon.gameObject.transform.position;
        
        public void UpdateView(string value) => _currentMoneyText.text = value;
    }
}