using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        
        public void SetAvatar(Sprite icon) => _avatar.sprite = icon;
        public void SetName(string planetName) => _name.text = planetName;
        public void SetPopulation(string populationCount) => _population.text = $"Population: {populationCount}";
        public void SetLevel(string level) => _level.text = $"Level: {level}";
        public void SetIncome(string income) => _income.text = $"Income: {income}";
        public void SetPrice(string price) => _price.text = $"Price: {price}";
        public void SetUpgradeAllowed(bool allowed) => _upgradeButton.interactable = allowed;
    }
}