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
        public event UnityAction OnUpgradeClicked
        {
            add => _upgradeButton.onClick.AddListener(value);
            remove => _upgradeButton.onClick.RemoveListener(value);
        }
        
        public event UnityAction OnCloseClicked
        {
            add => _closeButton.onClick.AddListener(value);
            remove => _closeButton.onClick.RemoveListener(value);
        }
        
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
        }

        private void Awake()
        {
            this.OnUpgradeClicked += _presenter.UpgradeClicked;
            this.OnCloseClicked += _presenter.CloseClicked;
            _presenter.OnUpdateView += this.UpdateView;
            _presenter.OnUpdateUpgradeButton += this.UpdateUpgradeButton;
            _presenter.OnCloseClicked += this.Hide;
            _presenter.OnUpgraded += this.SetLevel;
            _presenter.OnPopulationChanged += this.SetPopulation;
            _presenter.OnIncomeChanged += this.SetIncome;
            UpdateView();
        }

        private void OnDestroy()
        {
            this.OnUpgradeClicked -= _presenter.UpgradeClicked;
            this.OnCloseClicked -= _presenter.CloseClicked;
            _presenter.OnUpdateView -= this.UpdateView;
            _presenter.OnUpdateUpgradeButton -= this.UpdateUpgradeButton;
            _presenter.OnCloseClicked -= this.Hide;
            _presenter.OnUpgraded -= this.SetLevel;
            _presenter.OnPopulationChanged -= this.SetPopulation;
            _presenter.OnIncomeChanged -= this.SetIncome;
        }
        
        private void UpdateView()
        {
            SetAvatar(_presenter.Icon);
            SetName(_presenter.Name);
            SetPopulation(_presenter.Population.ToString());
            SetLevel($"{_presenter.Level} / {_presenter.MaxLevel}");
            SetIncome($"{_presenter.MinuteIncome} / sec");
            SetPrice($"{_presenter.Price}");
            SetUpgradeAllowed(_presenter.CanUpgrade);
            this.gameObject.SetActive(true);
        }
        
        private void UpdateUpgradeButton()
        {
            SetUpgradeAllowed(_presenter.CanUpgrade);
            SetPrice($"{_presenter.Price}");
        }
        
        private void Hide() => this.gameObject.SetActive(false);
        private void SetAvatar(Sprite icon) => _avatar.sprite = icon;
        private void SetName(string planetName) => _name.text = planetName;
        private void SetPopulation(string populationCount) => _population.text = $"Population: {populationCount}";
        private void SetLevel(string level) => _level.text = $"Level: {level}";
        private void SetIncome(string income) => _income.text = $"Income: {income}";
        private void SetPrice(string price) => _price.text = $"Price: {price}";
        private void SetUpgradeAllowed(bool allowed) => _upgradeButton.interactable = allowed;
    }
}