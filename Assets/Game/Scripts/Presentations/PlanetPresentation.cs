using System;
using Modules.Planets;
using UnityEngine;
using Zenject;
using R3;

namespace Game.Presenters
{
    public class PlanetPresentation : IInitializable, IDisposable
    {
        public event Action<Action> OnIncomeAnimation;
        

        public ReadOnlyReactiveProperty<Sprite> Icon => _icon;
        public ReadOnlyReactiveProperty<string> Price => _price;
        public ReadOnlyReactiveProperty<bool> IsIncomeReady => _isIncomeReady;
        public ReadOnlyReactiveProperty<bool> IsUnlocked => _isUnlocked;
        public ReadOnlyReactiveProperty<float> IncomeProgress => _incomeProgress;
        
        private readonly ReactiveProperty<Sprite> _icon = new();
        private readonly ReactiveProperty<string> _price = new();
        private readonly ReactiveProperty<bool> _isIncomeReady = new();
        private readonly ReactiveProperty<bool> _isUnlocked = new();
        private readonly ReactiveProperty<float> _incomeProgress = new();
        
        private readonly PlanetPopupPresentation _planetPopup;
        private readonly IPlanet _planet;
        
        public PlanetPresentation(Planet planet, PlanetPopupPresentation planetPopup)
        {
            _planetPopup = planetPopup;
            _planet = planet;
        }
        
        public void Initialize()
        {
            _icon.Value = _planet.GetIcon(_planet.IsUnlocked);
            _isUnlocked.Value = _planet.IsUnlocked;
            _price.Value = _planet.Price.ToString();
            _planet.OnIncomeChanged += this.IncomeTimeChanged;
            _planet.OnIncomeReady += this.IncomeReady;
        }
        
        public void Dispose()
        {
            _planet.OnIncomeChanged -= this.IncomeTimeChanged;
            _planet.OnIncomeReady -= this.IncomeReady;
        }
        
        public void PlanetClicked()
        {
            if (_planet.CanUnlockOrUpgrade)
            {
                _planet.Unlock();
                _icon.Value = _planet.GetIcon(_planet.IsUnlocked);
                _isUnlocked.Value = _planet.IsUnlocked;
                _isIncomeReady.Value = _planet.IsIncomeReady;
                _incomeProgress.Value = _planet.IncomeProgress;
            }
            
            if (_planet.IsUnlocked && _planet.IsIncomeReady)
            {
                this.OnIncomeAnimation?.Invoke(GatherIncome);   
            }
        }

        public void PlanetHold()
        {
            if (_planet.IsUnlocked)
            {
                _planetPopup.Show(_planet);
            }
        }

        private void GatherIncome()
        {
            _planet.GatherIncome();
            _incomeProgress.Value = _planet.IncomeProgress;
            _isIncomeReady.Value = _planet.IsIncomeReady;
        }
        
        private void IncomeReady(bool obj) => _isIncomeReady.Value = _planet.IsIncomeReady;

        private void IncomeTimeChanged(int remainingTime) => _incomeProgress.Value = _planet.IncomeProgress;

    }
}