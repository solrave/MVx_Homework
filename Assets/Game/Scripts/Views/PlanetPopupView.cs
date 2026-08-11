using Game.Presenters;
using TMPro;
using UnityEngine;
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
        
        private PlanetPopupPresentation _presentation;
        
        [Inject]
        public void Construct(PlanetPopupPresentation presentation)
        {
            _presentation = presentation;
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
            _closeButton.onClick.AddListener(_presentation.CloseClicked);
            _upgradeButton.onClick.AddListener(_presentation.UpgradeClicked);
            _presentation.OnUpdateView += this.UpdateView;
            _presentation.OnUpdateUpgradeButton += this.UpdateUpgradeButton;
            _presentation.OnCloseClicked += this.Hide;
            _presentation.OnUpgraded += this.SetLevel;
            _presentation.OnPopulationChanged += this.SetPopulation;
            _presentation.OnIncomeChanged += this.SetIncome;
        }

        private void Unsubscribe()
        {
            _closeButton.onClick.RemoveListener(_presentation.CloseClicked);
            _upgradeButton.onClick.RemoveListener(_presentation.UpgradeClicked);
            _presentation.OnUpdateView -= this.UpdateView;
            _presentation.OnUpdateUpgradeButton -= this.UpdateUpgradeButton;
            _presentation.OnCloseClicked -= this.Hide;
            _presentation.OnUpgraded -= this.SetLevel;
            _presentation.OnPopulationChanged -= this.SetPopulation;
            _presentation.OnIncomeChanged -= this.SetIncome;
        }

        private void UpdateView()
        {
            Show();
            SetAvatar(_presentation.Icon);
            SetName(_presentation.Name);
            SetPopulation(_presentation.Population.ToString());
            SetLevel($"Level: {_presentation.Level} / {_presentation.MaxLevel}");
            SetIncome($"Income: {_presentation.MinuteIncome}");
            SetPrice($"Price: {_presentation.Price}");
            SetUpgradeAllowed(_presentation.CanUpgrade);
        }
        
        private void UpdateUpgradeButton(bool allowed)
        {
            SetPrice(!allowed ? "Fully Upgraded" : $"Price: {_presentation.Price}");
            SetUpgradeAllowed(allowed);
        }
        
        private void Hide() => this.gameObject.SetActive(false);
        private void Show() => this.gameObject.SetActive(true);
        private void SetAvatar(Sprite icon) => _avatar.sprite = icon;
        private void SetName(string planetName) => _name.text = planetName;
        private void SetPopulation(string populationCount) => _population.text = populationCount;
        private void SetLevel(string level) => _level.text = level;
        private void SetIncome(string income) => _income.text = income;
        private void SetPrice(string price) => _price.text = price;
        private void SetUpgradeAllowed(bool allowed) => _upgradeButton.interactable = allowed;
    }
}