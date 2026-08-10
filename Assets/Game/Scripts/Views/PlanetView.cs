using UnityEngine;
using UnityEngine.UI;
using Modules.UI;
using TMPro;
using Zenject;
using Game.Presenters;
using Game.Views;
using Modules.Planets;

public class PlanetView : MonoBehaviour, IPlanetView
{
   [SerializeField] public string Name => _planetConfig.Name;
   [SerializeField] private PlanetConfig _planetConfig;
    
   [SerializeField] private Image _coin;
   [SerializeField] private CanvasGroup _progressGroup;
   [SerializeField] private CanvasGroup _priceGroup;
   [SerializeField] private TMP_Text _progressTime;
   [SerializeField] private Image _progressBar;
   [SerializeField] private TMP_Text _price;
   [SerializeField] private Image _planetIcon;
   [SerializeField] private Image _planetLock;
   [SerializeField] private SmartButton _button;
    
    private PlanetPresentation _presentation;
    private ParticleAnimator _coinAnimator;
    private MoneyView _moneyView;

    [Inject]
    public void Construct(ParticleAnimator coinAnimator, MoneyView moneyView)
    {
        _coinAnimator = coinAnimator;
        _moneyView = moneyView;
    }

    public void Initialize(PlanetPresentation presentation)
    {
        _presentation = presentation;
        HideCoin();
        HideProgressBar();
        SetPrice(_presentation.Price);
        SetIcon(_presentation.Icon);
        _button.OnHold += _presentation.PlanetHold;
        _button.OnClick += _presentation.PlanetClicked;
        _presentation.OnIncomeGathered += this.AnimateIncomeGathering;
        _presentation.OnUnlocked += this.PlanetUnlocked;
        _presentation.OnIncomeReady += IncomeReady;
        _presentation.OnIncomeTimeChanged += this.IncomeTimeChanged;
    }

    private void AnimateIncomeGathering()
    {
        _coinAnimator.Emit(_coin.rectTransform.position,
            _moneyView.CoinPosition);
    }

    private void OnDestroy()
    {
        if (_presentation != null)
        {
            _presentation.OnUnlocked -= this.PlanetUnlocked;
            _presentation.OnIncomeReady -= IncomeReady;
            _presentation.OnIncomeTimeChanged -= this.IncomeTimeChanged;
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
        if (_presentation.IsIncomeReady)
        {
            ShowCoin();
            HideProgressBar();
        }
        
        HideCoin();
        ShowProgressBar();
        SetProgressFill(incomeProgress);
        SetProgressText(Mathf.CeilToInt(remainingTime).ToString("F0:00"));
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
