using System;
using Modules.Planets;
using Modules.Utils;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPresenter : MonoBehaviour
    {
        public event Action<string, int> OnIncomeGathered;
        
        [SerializeField]
        private PlanetView _planetView;

        [SerializeField]
        public string Name;

        public Vector2 IncomeCoinPosition => _planetView.IncomeCoinPosition;
        
        private PlanetPopupPresenter _planetPopup;
        private IPlanet _planet;

        [Inject]
        private void Construct(PlanetPopupPresenter planetPopup,
            Countdown planetCountdown)
        {
            _planetPopup = planetPopup;
        }

        public void Initialize(IPlanet planet)
        { 
            _planet = planet;
            _planetView.SetIcon(_planet.GetIcon(false));
            _planetView.SetPrice(_planet.Price.ToString());
        }

        public void OnEnable()
        {
            _planetView.HideCoin();
            _planet.OnIncomeReady += IncomeReady;
            _planetView.OnPlanetHold += this.PlanetHold;
            _planetView.OnPlanetClicked += this.PlanetClicked;
            _planet.OnUnlocked += this.PlanetUnlocked;
            _planet.OnIncomeTimeChanged += this.IncomeTimeChanged;
            _planet.OnGathered += this.IncomeGathered;
        }

        public void OnDisable()
        {
            _planetView.HideCoin();
            _planet.OnIncomeReady -= IncomeReady;
            _planetView.OnPlanetHold -= this.PlanetHold;
            _planetView.OnPlanetClicked -= this.PlanetClicked;
            _planet.OnUnlocked -= this.PlanetUnlocked;
            _planet.OnIncomeTimeChanged -= this.IncomeTimeChanged;
            _planet.OnGathered -= this.IncomeGathered;
        }
        
        private void IncomeTimeChanged(float time)
        {
            _planetView.HideCoin();
            _planetView.ShowProgressBar();
            _planetView.SetProgressFill(_planet.IncomeProgress);
            _planetView.SetProgressText($"{time:0}");
        }
        
        private void IncomeGathered(int count)
        {
            this.OnIncomeGathered?.Invoke(Name, count);
            _planetView.HideCoin();
            _planetView.ShowProgressBar();
        }
        
        private void IncomeReady(bool obj)
        {
            _planetView.HideProgressBar();
            _planetView.ShowCoin(); 
        }
        
        private void PlanetClicked()
        {
            if (_planet.CanUnlockOrUpgrade)
                _planet.Unlock();
            
            if (_planet.IsUnlocked && _planet.IsIncomeReady)
            {
                _planet.GatherIncome();
            }
        }

        private void PlanetHold()
        {
            if (_planet.IsUnlocked)
                _planetPopup.Show(_planet);
        }
        
        private void PlanetUnlocked()
        {
            _planetView.SetIcon(_planet.GetIcon(true));
            _planetView.HideLock();
        }
    }
}