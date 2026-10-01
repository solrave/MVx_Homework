using Game.Presenters;
using R3;
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
        private TMP_Text _money;

        [SerializeField]
        private Image _coinIcon;
        
        private int _currentDisplayedCoins;
        private Coroutine _coroutine;
        private MoneyPresentation _presentation;
        private DisposableBag _disposableBag;

        [Inject]
        public void Construct(MoneyPresentation presentation)
        {
            _presentation = presentation;
        }

        private void OnEnable()
        {
            _money.text = _presentation.Money;
            _presentation.OnUpdateMoneyCount.Subscribe(UpdateMoneyView).AddTo(ref _disposableBag);
        }

        private void UpdateMoneyView(string money) => _money.text = money;

        private void OnDisable() => _disposableBag.Dispose();
    }
}