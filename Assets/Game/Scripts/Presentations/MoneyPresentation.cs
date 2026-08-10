using System;
using Modules.Money;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class MoneyPresentation : IInitializable, IDisposable
    {
        public event Action<string> OnUpdateView;
        public string Money => _moneyStorage.Money.ToString();
        private MoneyStorage _moneyStorage;

        public MoneyPresentation(MoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
        }

        public void Initialize()
        {
            _moneyStorage.OnMoneyChanged += this.MoneyChanged;
            _moneyStorage.OnMoneySpent += this.MoneySpent;
        }

        public void Dispose()
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
            OnUpdateView?.Invoke(newValue.ToString());
        }
    }
}