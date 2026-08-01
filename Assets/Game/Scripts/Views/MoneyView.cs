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
        public Vector2 IncomeCoinPosition => this._coinIcon.rectTransform.position;
        
        [SerializeField]
        private TMP_Text _currentMoneyText;

        [SerializeField]
        private Image _coinIcon;
        
        private MoneyPresenter _presenter;

        [Inject]
        public void Construct(MoneyPresenter presenter)
        {
            _presenter = presenter;
        }

        private void OnEnable()
        {
            UpdateView(_presenter.Money);
            _presenter.OnUpdateView += UpdateView;
        }

        private void OnDisable()
        {
            _presenter.OnUpdateView -= UpdateView;
        }

        private void UpdateView(string value) => _currentMoneyText.text = value;
    }
}