using System;
using Game.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Game.Views
{
    public class PlanetPopupView : MonoBehaviour
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _population;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _income;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _closeButton;
        
        private PlanetPopupPresenter _presenter;
        
        [Inject]
        public void Construct(PlanetPopupPresenter presenter)
        {
            _presenter = presenter;
            Subscribe();
        }

        private void Awake()
        {
            UpdateView();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }
        
        private void Subscribe()
        {
            _closeButton.onClick.AddListener(_presenter.CloseClicked);
            _upgradeButton.onClick.AddListener(_presenter.UpgradeClicked);
            _presenter.OnUpdateView += this.UpdateView;
            _presenter.OnUpdateUpgradeButton += this.UpdateUpgradeButton;
            _presenter.OnCloseClicked += this.Hide;
            _presenter.OnUpgraded += this.SetLevel;
            _presenter.OnPopulationChanged += this.SetPopulation;
            _presenter.OnIncomeChanged += this.SetIncome;
        }

        private void Unsubscribe()
        {
            _closeButton.onClick.RemoveListener(_presenter.CloseClicked);
            _upgradeButton.onClick.RemoveListener(_presenter.UpgradeClicked);
            _presenter.OnUpdateView -= this.UpdateView;
            _presenter.OnUpdateUpgradeButton -= this.UpdateUpgradeButton;
            _presenter.OnCloseClicked -= this.Hide;
            _presenter.OnUpgraded -= this.SetLevel;
            _presenter.OnPopulationChanged -= this.SetPopulation;
            _presenter.OnIncomeChanged -= this.SetIncome;
        }

        private void UpdateView()
        {
            Show();
            SetAvatar(_presenter.Icon);
            SetName(_presenter.Name);
            SetPopulation(_presenter.Population.ToString());
            SetLevel($"{_presenter.Level} / {_presenter.MaxLevel}");
            SetIncome($"{_presenter.MinuteIncome} / sec");
            SetPrice($"{_presenter.Price}");
            SetUpgradeAllowed(_presenter.CanUpgrade);
        }
        
        private void UpdateUpgradeButton()
        {
            SetUpgradeAllowed(_presenter.CanUpgrade);
            SetPrice($"{_presenter.Price}");
        }
        
        private void Hide() => this.gameObject.SetActive(false);
        private void Show() => this.gameObject.SetActive(true);
        private void SetAvatar(Sprite icon) => _avatar.sprite = icon;
        private void SetName(string planetName) => _name.text = planetName;
        private void SetPopulation(string populationCount) => _population.text = $"Population: {populationCount}";
        private void SetLevel(string level) => _level.text = $"Level: {level}";
        private void SetIncome(string income) => _income.text = $"Income: {income}";
        private void SetPrice(string price) => _price.text = $"Price: {price}";
        private void SetUpgradeAllowed(bool allowed) => _upgradeButton.interactable = allowed;
    }
}