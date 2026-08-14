using Game.Views;
using Modules.Planets;
using UnityEngine;

namespace Game.Presenters
{
    public class PlanetPopupPresenter : MonoBehaviour
    {
        [SerializeField]
        private PlanetPopupView _planetPopupView;
        
        private IPlanet _planet;
        
        public void Show(IPlanet planet)
        {
            this.gameObject.SetActive(true);
            _planet = planet;
            _planetPopupView.OnUpgradeClicked += this.UpgradeClicked;
            _planetPopupView.OnCloseClicked += this.CloseClicked;
            _planet.OnUpgraded += this.Upgraded;
            _planet.OnIncomeChanged += this.IncomeChanged;
            _planet.OnPopulationChanged += this.PopulationChanged;
            UpdateView();
        }

        private void Hide()
        {
            _planetPopupView.OnUpgradeClicked -= this.UpgradeClicked;
            _planetPopupView.OnCloseClicked -= this.CloseClicked;
            _planet.OnUpgraded -= this.Upgraded;
            _planet.OnIncomeChanged -= this.IncomeChanged;
            _planet.OnPopulationChanged -= this.PopulationChanged;
            this.gameObject.SetActive(false);
            _planet = null;
        }

        private void UpdateView()
        {
            _planetPopupView.SetAvatar(_planet.GetIcon(true));
            _planetPopupView.SetName(_planet.Name);
            this.PopulationChanged(_planet.Population);
            this.Upgraded(_planet.Level / _planet.MaxLevel);
            this.IncomeChanged(_planet.MinuteIncome);
            _planetPopupView.SetPrice($"Price: {_planet.Price}");
            _planetPopupView.SetUpgradeAllowed(_planet.CanUpgrade);
            UpdateButton(_planet.CanUpgrade);
        }

        private void PopulationChanged(int num)
            => _planetPopupView.SetPopulation($"Population: {num.ToString()}");

        private void IncomeChanged(int income)
            => _planetPopupView.SetIncome($"Income: {income.ToString()} / sec");

        private void Upgraded(int level) 
            => _planetPopupView.SetLevel($"Level: {_planet.Level} / {_planet.MaxLevel}");

        private void CloseClicked() => this.Hide();
        
        private void UpgradeClicked()
        {
            if (_planet.IsMaxLevel)
                return;
            
            _planet.Upgrade();
            UpdateButton(_planet.CanUpgrade);
        }

        private void UpdateButton(bool upgradeAllowed)
        {
            _planetPopupView.SetUpgradeAllowed(upgradeAllowed);
            _planetPopupView.SetPrice(
                !_planet.IsMaxLevel
                ? $"{_planet.Price}"
                : "Fully upgraded");
            if (_planet.IsMaxLevel)
                _planetPopupView.SetUpgradeAllowed(false);
        }
    }
}