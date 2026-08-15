using System;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPresentation : IInitializable, IDisposable
    {
        public event Action<Action> OnIncomeGathered;
        public event Action OnUnlocked;
        public event Action<bool> OnIncomeReady;
        public event Action<float, float> OnIncomeTimeChanged;
        
        public Sprite Icon => _planet.GetIcon(_planet.IsUnlocked);
        public string Price => _planet.Price.ToString();
        public bool IsIncomeReady => _planet.IsIncomeReady;
        
        private readonly PlanetPopupPresentation _planetPopup;
        private readonly IPlanet _planet;
        
        public PlanetPresentation(Planet planet, PlanetPopupPresentation planetPopup)
        {
            _planetPopup = planetPopup;
            _planet = planet;
        }
        
        public void Initialize()
        {
            _planet.OnUnlocked += this.PlanetUnlocked;
            _planet.OnIncomeReady += IncomeReady;
            _planet.OnIncomeTimeChanged += this.IncomeTimeChanged;
        }

        public void Dispose()
        {
            _planet.OnUnlocked -= this.PlanetUnlocked;
            _planet.OnIncomeReady -= IncomeReady;
            _planet.OnIncomeTimeChanged -= this.IncomeTimeChanged;
        }
        
        public void PlanetClicked()
        {
            if (_planet.CanUnlockOrUpgrade)
            {
                _planet.Unlock();
            }
            
            if (_planet.IsUnlocked && _planet.IsIncomeReady)
            {
                this.OnIncomeGathered?.Invoke(GatherIncome);   
            }
        }

        public void PlanetHold()
        {
            if (_planet.IsUnlocked)
            {
                _planetPopup.Show(_planet);
            }
        }

        private void GatherIncome() => _planet.GatherIncome();
        
        private void IncomeTimeChanged(float time) => this.OnIncomeTimeChanged?.Invoke(_planet.IncomeProgress, time);

        private void IncomeReady(bool state) => this.OnIncomeReady?.Invoke(state);
        
        private void PlanetUnlocked() => this.OnUnlocked?.Invoke();
    }
}