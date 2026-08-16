using System;
using UnityEngine;
using UnityEngine.UI;
using Modules.UI;
using TMPro;
using Zenject;
using Game.Presenters;
using Game.Views;
using Modules.Planets;
using R3;

public class PlanetView : MonoBehaviour, IPlanetView
{ 
    private const string FORMAT = @"mm\:ss";
    public string Name => _planetConfig.Name;
   
    [SerializeField] private PlanetConfig _planetConfig;
    
    [SerializeField] private Image _coin;
    [SerializeField] private GameObject _progressGroup;
    [SerializeField] private GameObject _priceGroup;
    [SerializeField] private TMP_Text _progressTime;
    [SerializeField] private Image _progressBar;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Image _planetIcon;
    [SerializeField] private Image _planetLock;
    [SerializeField] private SmartButton _button;
    [SerializeField] private float _duration = 1f;
    
    private PlanetPresentation _presentation;
    private ParticleAnimator _coinAnimator;
    private MoneyView _moneyView;
    
    private DisposableBag _disposableBag;

    [Inject]
    public void Construct(ParticleAnimator coinAnimator, MoneyView moneyView)
    {
        _coinAnimator = coinAnimator;
        _moneyView = moneyView;
    }

    public void Initialize(PlanetPresentation presentation)
    {
        _presentation = presentation;
        
        _button.OnHold += _presentation.PlanetHold;
        _button.OnClick += _presentation.PlanetClicked;
        
        _presentation.OnIncomeAnimation += this.AnimateIncome;
        
        _presentation.Price.Subscribe(SetPriceText).AddTo(ref _disposableBag);
        _presentation.Icon.Subscribe(SetIcon).AddTo(ref _disposableBag);
        _presentation.IsUnlocked.Subscribe(SetLockedState).AddTo(ref _disposableBag);
        _presentation.IsIncomeReady.Subscribe(SetIncomeUI).AddTo(ref _disposableBag);
        _presentation.IncomeProgress.Subscribe(FillProgress).AddTo(ref _disposableBag);
    }

    private void OnDestroy()
    {
        if (_presentation == null) return;
        
        _presentation.OnIncomeAnimation -= this.AnimateIncome;
        _disposableBag.Dispose();
    }
    
    private void InitializeView()
    {
        HideCoin();
        HideProgressBar();
    }

    private void AnimateIncome(Action callback)
    {
        HideCoin();
        _coinAnimator.Emit(_coin.rectTransform.position, 
            _moneyView.CoinPosition, 1f, callback);
    }
    
    private void FillProgress(float incomeProgress)
    {
        if (!_presentation.IsUnlocked.CurrentValue) return; 
        
        ShowProgressBar();
        //var time = TimeSpan.FromSeconds(Mathf.CeilToInt(incomeProgress));
        _progressBar.fillAmount = 1f - incomeProgress;
        _progressTime.SetText(incomeProgress.ToString(FORMAT));
    }
    
    private void SetIncomeUI(bool isReady)
    {
        if (!_presentation.IsUnlocked.CurrentValue) return; 
        
        if (isReady)
        {
            HideProgressBar();
            ShowCoin(); 
        }
        else
        {
            ShowProgressBar();
            HideCoin(); 
        }
    }

    private void SetLockedState(bool unlocked)
    {
        if (!unlocked)
        {
            ShowLock();
            ShowPrice();
            HideProgressBar();
            HideCoin();
        }
        else
        {
            HideLock();
            HidePrice();
        }
    }
    
    private void SetPriceText(string text) => _priceText.text = text;
    private void SetIcon(Sprite icon) => _planetIcon.sprite = icon;
    private void ShowCoin() => _coin.enabled = true;
    private void HideCoin() => _coin.enabled = false;
    private void HideProgressBar() => _progressGroup.SetActive(false);
    private void ShowProgressBar() => _progressGroup.SetActive(true);
    private void ShowLock() => _planetLock.gameObject.SetActive(true);
    private void HideLock() => _planetLock.gameObject.SetActive(false);
    private void ShowPrice() => _priceGroup.gameObject.SetActive(true);
    private void HidePrice() => _priceGroup.gameObject.SetActive(false);
}
