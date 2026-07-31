using System;
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
        }
    }
}