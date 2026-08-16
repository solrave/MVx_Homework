using System;
using Modules.Planets;
using UnityEngine;
using R3;

namespace Game.Presenters
{
    public class PlanetPopupPresentation
    {
        public event Action OnShow;
        public ReadOnlyReactiveProperty<Sprite> Icon => _icon;
        public ReadOnlyReactiveProperty<string> Name => _name;
        public ReadOnlyReactiveProperty<string> Population => _population;
        public ReadOnlyReactiveProperty<string> Level => _level;
        public ReadOnlyReactiveProperty<string> Income => _income;
        public ReadOnlyReactiveProperty<string> Price => _price;
        public ReadOnlyReactiveProperty<bool> IsMaxLevel => _isMaxLevel;
        public ReadOnlyReactiveProperty<bool> CanUpgrade => _canUpgrade;
        public ReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        
        private readonly ReactiveProperty<Sprite> _icon = new();
        private readonly ReactiveProperty<string> _name = new();
        private readonly ReactiveProperty<string> _population = new();
        private readonly ReactiveProperty<string> _level = new();
        private readonly ReactiveProperty<string> _income = new();
        private readonly ReactiveProperty<string> _price = new();
        private readonly ReactiveProperty<bool> _isMaxLevel = new();
        private readonly ReactiveProperty<bool> _canUpgrade = new();
        private readonly ReactiveProperty<bool> _isVisible = new();
        
        private IPlanet _planet;
        
        public void Show(IPlanet planet)
        {
            _planet = planet;
            _planet.OnUpgraded += OnUpgraded;
            _icon.Value = _planet.GetIcon(_planet.IsUnlocked);
            _name.Value = _planet.Name;
            _population.Value = $"Population: {_planet.Population}";
            _level.Value = $"Level: {_planet.Level} / {_planet.MaxLevel}";
            _income.Value = $"Income: {_planet.MinuteIncome}";
            _price.Value = $"Price: {_planet.Price.ToString()}";
            _isMaxLevel.Value = _planet.IsMaxLevel;
            _canUpgrade.Value = _planet.CanUnlockOrUpgrade;
            _isVisible.Value = true;
            OnShow?.Invoke();
        }

        private void OnUpgraded(int obj)
        {
            _population.Value = $"Population: {_planet.Population}";
            _level.Value = $"Level: {_planet.Level} / {_planet.MaxLevel}";
            _income.Value = $"Income: {_planet.MinuteIncome}";
            _price.Value = $"Price: {_planet.Price.ToString()}";
            _isMaxLevel.Value = _planet.IsMaxLevel;
            _canUpgrade.Value = _planet.CanUnlockOrUpgrade;
        }

        private void Hide()
        {
            _planet.OnUpgraded -= OnUpgraded;
            _isVisible.Value = false;
            _planet = null;
        }
        
        public void CloseClicked()
        {
            this.Hide();
        }

        public void UpgradeClicked()
        {
            if (!_planet.CanUpgrade) return;
            
            _planet.Upgrade();
        }
    }
}