using Modules.Planets;
using Modules.UI;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPresenter : MonoBehaviour
    {
        public string Name => _config.Name;
        
        [SerializeField]
        private PlanetView _planetView;

        [SerializeField]
        private PlanetConfig _config;
        
        private PlanetPopupPresenter _planetPopup;
        private IPlanet _planet;
        private ParticleAnimator _coinAnimator;
        private MoneyPresenter _moneyPresenter;

        [Inject]
        private void Construct(ParticleAnimator coinAnimator,
            PlanetPopupPresenter planetPopup,
            MoneyPresenter moneyPresenter)
        {
            _planetPopup = planetPopup;
            _coinAnimator = coinAnimator;
            _moneyPresenter = moneyPresenter; 
        }

        public void Initialize(IPlanet planet)
        { 
            _planet = planet;
            _planetView.SetIcon(_planet.GetIcon(false));
            _planetView.SetPrice(_planet.Price.ToString());
        }

        public void Start()
        {
            _planetView.HideCoin();
            _planetView.HideProgressBar();
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
            _planetView.SetProgressText($"{time: 00:00}");
        }
        
        private void IncomeGathered(int count)
        {
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
                AnimateIncome();
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
        
        private void AnimateIncome()
        {
            _coinAnimator.Emit(_planetView.IncomeCoinPosition,
                _moneyPresenter.IncomeCoinPosition);
        }
    }
}