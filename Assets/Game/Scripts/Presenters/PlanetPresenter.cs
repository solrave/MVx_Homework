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
            _planetView.HideProgressGroup();
            _planet.OnIncomeReady += IncomeReady;
            _planetView.OnPlanetHold += this.PlanetHold;
            _planetView.OnPlanetClicked += this.PlanetClicked;
            _planet.OnUnlocked += this.PlanetUnlocked;
            _planet.OnIncomeTimeChanged += this.IncomeTimeChanged;
        }

        public void OnDisable()
        {
            _planetView.HideCoin();
            _planet.OnIncomeReady -= IncomeReady;
            _planetView.OnPlanetHold -= this.PlanetHold;
            _planetView.OnPlanetClicked -= this.PlanetClicked;
            _planet.OnUnlocked -= this.PlanetUnlocked;
            _planet.OnIncomeTimeChanged -= this.IncomeTimeChanged;
        }

        private void IncomeTimeChanged(float time)
        {
            _planetView.HideCoin();
            _planetView.ShowProgressGroup();
            _planetView.SetProgressFill(1f - _planet.IncomeProgress);
            _planetView.SetProgressText($"{time: 00:00}");
        }
        
        private void GatherIncome()
        {
            _planetView.HideCoin();
            if (_planet.IsUnlocked)
                _planetView.ShowProgressGroup();
            
            _planet.GatherIncome();
        }
        
        private void IncomeReady(bool isReady)
        {
            if (isReady)
            {
                _planetView.HideProgressGroup();
                _planetView.ShowCoin(); 
            }
            else
            {
                _planetView.ShowProgressGroup();
                _planetView.HideCoin(); 
            }
        }
        
        private void PlanetClicked()
        {
            if (_planet.CanUnlockOrUpgrade)
                _planet.Unlock();

            if (_planet.IsUnlocked && _planet.IsIncomeReady)
            {
                _planetView.HideCoin();
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
            _planetView.HidePriceGroup();
        }
        
        private void AnimateIncome()
        {
            _coinAnimator.Emit(_planetView.IncomeCoinPosition,
                _moneyPresenter.IncomeCoinPosition,1f, this.GatherIncome);
        }
    }
}