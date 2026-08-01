using System;
using Modules.Planets;
using Modules.Utils;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPresenter : IInitializable, IDisposable
    {
        public event Action OnIncomeGathered;
        public event Action<Sprite> OnUnlocked;
        public event Action<bool> OnIncomeReady;
        public event Action<float, float> OnIncomeTimeChanged;
        
        public Sprite Icon => _planet.GetIcon(_planet.IsUnlocked);
        public string Price => _planet.Price.ToString();
        public bool IsIncomeReady => _planet.IsIncomeReady;
        
        private PlanetPopupPresenter _planetPopup;
        private IPlanet _planet;
        private MoneyPresenter _moneyPresenter;
        
        public PlanetPresenter(Planet planet, PlanetPopupPresenter planetPopup, MoneyPresenter moneyPresenter)
        {
            _planetPopup = planetPopup;
            _moneyPresenter = moneyPresenter;
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
                OnUnlocked?.Invoke(_planet.GetIcon(true));
            }
            
            if (_planet.IsUnlocked && _planet.IsIncomeReady)
            {
                _planet.GatherIncome();
                OnIncomeGathered?.Invoke();
            }
        }

        public void PlanetHold()
        {
            if (_planet.IsUnlocked)
                _planetPopup.Show(_planet);
        }
        
        private void IncomeTimeChanged(float time)
        {
            this.OnIncomeTimeChanged?.Invoke(_planet.IncomeProgress, time);
        }

        private void IncomeReady(bool state) => this.OnIncomeReady?.Invoke(state);
        
        private void PlanetUnlocked() => this.OnUnlocked?.Invoke(_planet.GetIcon(true));
    }
}