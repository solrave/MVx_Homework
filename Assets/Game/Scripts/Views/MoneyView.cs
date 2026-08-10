using System;
using Game.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        public Vector3 CoinPosition => this._coinIcon.rectTransform.position;
        
        [SerializeField]
        private TMP_Text _currentMoneyText;

        [SerializeField]
        private Image _coinIcon;
        
        private MoneyPresentation _presentation;

        [Inject]
        public void Construct(MoneyPresentation presentation)
        {
            _presentation = presentation;
        }

        private void OnEnable()
        {
            UpdateView(_presentation.Money);
            _presentation.OnUpdateView += UpdateView;
        }

        private void OnDisable()
        {
            _presentation.OnUpdateView -= UpdateView;
        }

        private void UpdateView(string value) => _currentMoneyText.text = value;
    }
}