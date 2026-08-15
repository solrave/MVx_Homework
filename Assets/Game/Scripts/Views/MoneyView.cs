using System.Collections;
using Game.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        public Vector3 CoinPosition => this._coinIcon.rectTransform.position;
        
        [SerializeField]
        private TMP_Text _money;

        [SerializeField]
        private Image _coinIcon;

        [SerializeField] 
        private float _duration = 1f;

        private int _currentDisplayedCoins;
        private Coroutine _coroutine;
        private MoneyPresentation _presentation;

        [Inject]
        public void Construct(MoneyPresentation presentation)
        {
            _presentation = presentation;
        }

        private void OnEnable()
        {
            _money.text = _presentation.Money;
            _presentation.OnUpdateView += UpdateView;
        }

        private void OnDisable() => _presentation.OnUpdateView -= UpdateView;

        private void UpdateView(int newValue, int prevValue)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            StartCoroutine(IncomeAnimation(prevValue, newValue));
        }
        
        private IEnumerator IncomeAnimation(int startValue, int targetValue)  
        {  
            float elapsed = 0f;  
  
            while (elapsed < _duration)  
            {       
                elapsed += Time.deltaTime;  
                float progress = elapsed / _duration;  
                _currentDisplayedCoins = (int)Mathf.Lerp(startValue, targetValue, progress);  
                _money.text = _currentDisplayedCoins.ToString();  
                yield return null;  
            }   
            
            _currentDisplayedCoins = targetValue;  
            _money.text = _currentDisplayedCoins.ToString();  
            _coroutine = null;  
        }
    }
}