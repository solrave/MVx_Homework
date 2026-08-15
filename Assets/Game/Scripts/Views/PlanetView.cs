using System;
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
        InitializeView();
        _button.OnHold += _presentation.PlanetHold;
        _button.OnClick += _presentation.PlanetClicked;
        
        _presentation.OnIncomeGathered += this.AnimateIncome;
        _presentation.OnUnlocked += this.UnlockPlanet;
        _presentation.OnIncomeReady += SetIncomeUI;
        _presentation.OnIncomeTimeChanged += this.ShowProgressBar;
    }

    private void OnDestroy()
    {
        if (_presentation == null) return;
        
        _presentation.OnIncomeGathered -= this.AnimateIncome;
        _presentation.OnUnlocked -= this.UnlockPlanet;
        _presentation.OnIncomeReady -= SetIncomeUI;
        _presentation.OnIncomeTimeChanged -= this.ShowProgressBar;
    }
    
    private void InitializeView()
    {
        HideCoin();
        HideProgressBar();
        _priceText.text = _presentation.Price;
        _planetIcon.sprite = _presentation.Icon;
    }

    private void AnimateIncome(Action callback)
    {
        HideCoin();
        _coinAnimator.Emit(_coin.rectTransform.position, 
            _moneyView.CoinPosition, 1f, callback);
    }
    
    private void ShowProgressBar(float incomeProgress, float remainingTime)
    {
        var time = TimeSpan.FromSeconds(Mathf.CeilToInt(remainingTime));
        HideCoin();
        ShowProgressBar();
        _progressBar.fillAmount = 1f - incomeProgress;
        _progressTime.SetText(time.ToString(FORMAT));
    }
    
    private void SetIncomeUI(bool isReady)
    {
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

    private void UnlockPlanet()
    {
        _planetIcon.sprite = _presentation.Icon;
        _planetLock.gameObject.SetActive(false);
        _priceGroup.SetActive(false);
        ShowProgressBar();
    }
    
    private void ShowCoin() => _coin.enabled = true;
    private void HideCoin() => _coin.enabled = false;
    private void HideProgressBar() => _progressGroup.SetActive(false);
    private void ShowProgressBar() => _progressGroup.SetActive(true);
}
