using System;
using Modules.Money;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPopupPresenter
    {
        public event Action OnUpdateView;
        public event Action<string> OnUpgraded;
        public event Action<string> OnIncomeChanged;
        public event Action<string> OnPopulationChanged;
        public event Action OnUpdateUpgradeButton;
        public event Action OnCloseClicked;
        public string Name => _planet.Name;
        public Sprite Icon => _planet.GetIcon(_planet.IsUnlocked);
        public int Population => _planet.Population;
        public int Level => _planet.Level;
        public int MaxLevel => _planet.MaxLevel;
        public int MinuteIncome => _planet.MinuteIncome;
        public string Price => _planet.Price.ToString();
        public bool CanUpgrade => _planet.CanUnlockOrUpgrade;
        
        private IPlanet _planet;
        private readonly IMoneyStorage _moneyStorage;

        public PlanetPopupPresenter(IMoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
        }

        public void Show(IPlanet planet)
        {
            _planet = planet;
            _planet.OnUpgraded += this.Upgraded;
            _planet.OnPopulationChanged += this.PopulationChanged;
            _planet.OnIncomeChanged += this.IncomeChanged;
            _moneyStorage.OnMoneyChanged += this.OnMoneyChanged;
            OnUpdateView?.Invoke();
        }

        private void Hide()
        {
            _planet.OnUpgraded -= this.Upgraded;
            _planet.OnPopulationChanged -= this.PopulationChanged;
            _planet.OnIncomeChanged -= this.IncomeChanged;
            _moneyStorage.OnMoneyChanged -= this.OnMoneyChanged;
            _planet = null;
        }
        
        public void CloseClicked()
        {
            OnCloseClicked?.Invoke();
            this.Hide();
        }

        public void UpgradeClicked()
        {
            _planet.Upgrade();
            OnUpdateUpgradeButton?.Invoke();
        }

        private void OnMoneyChanged(int newValue, int prevValue)
        {
            this.OnUpdateUpgradeButton?.Invoke();
        }

        private void PopulationChanged(int num) => this.OnPopulationChanged?.Invoke(num.ToString());

        private void IncomeChanged(int income) => this.OnIncomeChanged?.Invoke(income.ToString());

        private void Upgraded(int level) => this.OnUpgraded?.Invoke($"{level} / {_planet.MaxLevel}");
    }
}