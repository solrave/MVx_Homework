using System;
using Game.Views;
using Modules.Money;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPopupPresenter : MonoBehaviour
    {
        [SerializeField]
        private PlanetPopupView _planetPopupView;
        
        private IPlanet _planet;

        private void Update()
        {
            UpdateUpgradeButton();
        }

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
            _planetPopupView.SetPopulation(_planet.Population.ToString());
            _planetPopupView.SetLevel($"{_planet.Level} / {_planet.MaxLevel}");
            _planetPopupView.SetIncome($"{_planet.MinuteIncome} / sec");
            _planetPopupView.SetPrice($"{_planet.Price}");
            _planetPopupView.SetUpgradeAllowed(_planet.CanUpgrade);
        }

        private void PopulationChanged(int num) => _planetPopupView.SetPopulation($"{num.ToString()}");

        private void IncomeChanged(int income) => _planetPopupView.SetIncome($"{income.ToString()} / sec");

        private void Upgraded(int level) => _planetPopupView.SetLevel($"{_planet.Level} / {_planet.MaxLevel}");

        private void CloseClicked() => this.Hide();

        private void UpgradeClicked()
        {
            _planet.Upgrade();
            UpdateUpgradeButton();
        }

        private void UpdateUpgradeButton()
        {
            _planetPopupView.SetUpgradeAllowed(_planet.CanUpgrade);
            _planetPopupView.SetPrice($"{_planet.Price}");
        }
    }
}