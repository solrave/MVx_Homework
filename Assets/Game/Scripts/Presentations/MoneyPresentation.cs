using System;
using Modules.Money;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class MoneyPresentation : IInitializable, IDisposable, ITickable
    {
        public ReactiveCommand<string> OnUpdateMoneyCount = new();
        public string Money => _moneyStorage.Money.ToString();
        private readonly MoneyStorage _moneyStorage;
        private readonly float _duration = 1f;
        private int _currentDisplayedCoins;
        private bool _animationRequested;
        private int _currentMoney;
        private int _newMoney;
        private float _elapsed;
        private float _progress;

        public MoneyPresentation(MoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
        }

        public void Initialize() => _moneyStorage.OnMoneyChanged += this.ChangeMoney;

        public void Dispose() => _moneyStorage.OnMoneyChanged -= this.ChangeMoney;

        private void ChangeMoney(int newValue, int lastValue)
        {
            _currentMoney = lastValue;
            _newMoney = newValue;
            _animationRequested = true;
            _elapsed = 0f;
        }

        public void Tick()
        {
            this.ProcessCountAnimation();
        }

        private void ProcessCountAnimation()
        {
            if (!_animationRequested) return;
            
            _elapsed += Time.deltaTime;  
            _progress = _elapsed / _duration;  
            _currentDisplayedCoins = (int)Mathf.Lerp(_currentMoney, _newMoney, _progress);  
            OnUpdateMoneyCount?.Execute(_currentDisplayedCoins.ToString());

            if (_progress >= 1f)
            {
                _currentDisplayedCoins = _newMoney;  
                OnUpdateMoneyCount?.Execute(_currentDisplayedCoins.ToString());
                _newMoney = 0;
                _animationRequested = false;
            }
        }
    }
}