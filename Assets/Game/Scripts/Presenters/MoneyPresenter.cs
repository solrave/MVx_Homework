using System;
using System.Collections;
using Game.Views;
using Modules.Money;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class MoneyPresenter : MonoBehaviour
    {
        [SerializeField]
        private MoneyView _moneyView;
        
        private MoneyStorage _moneyStorage;
        private int _currentDisplayedCoins = 0;
        private Coroutine _countCoroutine;
        
        public Vector2 IncomeCoinPosition => _moneyView.IncomeCoinPosition;

        [Inject]
        public void Construct(MoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
            _moneyView.UpdateView(_moneyStorage.Money.ToString());
        }

        private void OnEnable()
        {
            _moneyStorage.OnMoneyChanged += this.MoneyChanged;
            _moneyStorage.OnMoneySpent += this.MoneySpent;
        }

        private void OnDisable()
        {
            _moneyStorage.OnMoneyChanged -= this.MoneyChanged;
            _moneyStorage.OnMoneySpent -= this.MoneySpent;
        }

        private void MoneySpent(int newValue, int range)
        {
           //Money Animation
        }

        private void MoneyChanged(int newValue, int prevValue)
        { 
            _moneyView.UpdateView(newValue.ToString());
            StartCoroutine(CountCoinsCoroutine(prevValue, newValue));
        }
        
        private IEnumerator CountCoinsCoroutine(int startValue, int targetValue)
        {
            float elapsed = 0f;

            while (elapsed < _moneyView.Duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / _moneyView.Duration;
                
                _currentDisplayedCoins = (int)Mathf.Lerp(startValue, targetValue, progress);
                _moneyView.UpdateView(_currentDisplayedCoins.ToString());
            
                yield return null;
            }
            
            _currentDisplayedCoins = targetValue;
            _moneyView.UpdateView(_currentDisplayedCoins.ToString());
            
            _countCoroutine = null;
        }
    }
}