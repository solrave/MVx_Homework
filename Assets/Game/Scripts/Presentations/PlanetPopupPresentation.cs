using System;
using Modules.Money;
using Modules.Planets;
using UnityEngine;
using R3;

namespace Game.Presenters
{
    public class PlanetPopupPresentation
    {
        //public ReactiveProperty<string> NameProperty { get; } = new ReactiveProperty<string>("");
        public event Action OnUpdateView;
        public event Action<string> OnUpgraded;
        public event Action<string> OnIncomeChanged;
        public event Action<string> OnPopulationChanged;
        public event Action<bool> OnRefreshButton;
        public event Action OnCloseClicked;
        
        public string Name => _planet.Name;
        public Sprite Icon => _planet.GetIcon(_planet.IsUnlocked);
        public string Population => $"Population: {_planet.Population}";
        public string Level => $"Level: {_planet.Level} / {_planet.MaxLevel}";
        public string Income => $"Income: {_planet.MinuteIncome}";
        public string Price => $"Price: {_planet.Price.ToString()}";
        public bool IsMaxLevel => _planet.IsMaxLevel;
        public bool CanUpgrade => _planet.CanUnlockOrUpgrade;
        private IPlanet _planet;

        

        public void Show(IPlanet planet)
        {
            _planet = planet;
            _planet.OnUpgraded += this.Upgraded;
            _planet.OnPopulationChanged += this.PopulationChanged;
            _planet.OnIncomeChanged += this.IncomeChanged;
            OnUpdateView?.Invoke();
        }

        private void Hide()
        {
            _planet.OnUpgraded -= this.Upgraded;
            _planet.OnPopulationChanged -= this.PopulationChanged;
            _planet.OnIncomeChanged -= this.IncomeChanged;
            _planet = null;
        }
        
        public void CloseClicked()
        {
            OnCloseClicked?.Invoke();
            this.Hide();
        }

        public void UpgradeClicked()
        {
            if (!_planet.CanUpgrade) return;
            
            _planet.Upgrade();
            OnRefreshButton?.Invoke(_planet.CanUpgrade);
        }

        private void PopulationChanged(int num) => this.OnPopulationChanged?.Invoke(Population);

        private void IncomeChanged(int income) => this.OnIncomeChanged?.Invoke(Income);

        private void Upgraded(int level) => this.OnUpgraded?.Invoke(Level);
    }
}