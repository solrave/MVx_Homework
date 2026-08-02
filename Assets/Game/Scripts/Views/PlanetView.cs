using UnityEngine;
using UnityEngine.UI;
using Modules.UI;
using TMPro;
using Zenject;
using Game.Presenters;
using Game.Views;

public class PlanetView : MonoBehaviour
{
   [SerializeField] public string Name;
    
    [SerializeField] private Image _coin;
    [SerializeField] private CanvasGroup _progressGroup;
    [SerializeField] private CanvasGroup _priceGroup;
    [SerializeField] private TMP_Text _progressTime;
    [SerializeField] private Image _progressBar;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Image _planetIcon;
    [SerializeField] private Image _planetLock;
    [SerializeField] private SmartButton _button;
    
    private PlanetPresenter _presenter;
    private ParticleAnimator _coinAnimator;
    private Vector2 _moneyViewCoinPosition;

    [Inject]
    public void Construct(ParticleAnimator coinAnimator, MoneyView moneyView)
    {
        _coinAnimator = coinAnimator;
        _moneyViewCoinPosition = moneyView.IncomeCoinPosition;
        Debug.Log($"COIN POSITION: {_moneyViewCoinPosition}");
    }

    public void Initialize(PlanetPresenter presenter)
    {
        _presenter = presenter;
        HideCoin();
        HideProgressBar();
        SetPrice(_presenter.Price);
        SetIcon(_presenter.Icon);
        _button.OnHold += _presenter.PlanetHold;
        _button.OnClick += _presenter.PlanetClicked;
        _presenter.OnIncomeGathered += this.AnimateIncomeGathering;
        _presenter.OnUnlocked += this.PlanetUnlocked;
        _presenter.OnIncomeReady += IncomeReady;
        _presenter.OnIncomeTimeChanged += this.IncomeTimeChanged;
    }

    private void AnimateIncomeGathering()
    {
        _coinAnimator.Emit(_coin.rectTransform.position,
           new Vector2(45,420) );//_moneyViewCoinPosition
    }

    private void OnDestroy()
    {
        if (_presenter != null)
        {
            _presenter.OnUnlocked -= this.PlanetUnlocked;
            _presenter.OnIncomeReady -= IncomeReady;
            _presenter.OnIncomeTimeChanged -= this.IncomeTimeChanged;
        }
    }

    private void ShowCoin() => _coin.enabled = true;
    private void HideCoin() => _coin.enabled = false;
    private void SetProgressText(string text) => this._progressTime.SetText(text);
    private void SetProgressFill(float value) => this._progressBar.fillAmount = value;
    private void SetPrice(string text) => this._price.SetText(text);
    private void SetIcon(Sprite icon) => this._planetIcon.sprite = icon;
    private void HideLock() => _planetLock.gameObject.SetActive(false);
    private void HideProgressBar() => _progressGroup.alpha = 0;
    private void ShowProgressBar() => _progressGroup.alpha = 1;
    private void HidePrice() => this._priceGroup.alpha = 0;
    
    private void IncomeTimeChanged(float incomeProgress, float remainingTime)
    {
        if (_presenter.IsIncomeReady)
        {
            ShowCoin();
            HideProgressBar();
        }
        
        HideCoin();
        ShowProgressBar();
        SetProgressFill(incomeProgress);
        SetProgressText(Mathf.CeilToInt(remainingTime).ToString("F0"));
    }
    
    private void IncomeReady(bool obj)
    {
        HideProgressBar();
        ShowCoin(); 
    }

    private void PlanetUnlocked(Sprite icon)
    {
        SetIcon(icon);
        HideLock();
        HidePrice();
        ShowProgressBar();
        ShowCoin();
    }
}
