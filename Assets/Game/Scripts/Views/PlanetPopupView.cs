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
        public void SetName(string name) => _name.text = name;
        public void SetPopulation(string name) => _population.text = $"Population: {name}";
        public void SetLevel(string name) => _level.text = $"Level: {name}";
        public void SetIncome(string name) => _income.text = $"Income: {name}";
        public void SetPrice(string name) => _price.text = $"Price: {name}";
        public void SetUpgradeAllowed(bool allowed) => _upgradeButton.interactable = allowed;
    }
}