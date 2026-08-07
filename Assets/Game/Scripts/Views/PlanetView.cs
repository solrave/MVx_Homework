using System;
using Modules.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlanetView : MonoBehaviour
{
    public event Action OnPlanetClicked
    {
        add => _button.OnClick += value;
        remove => _button.OnClick -= value;
    }
    
    public event Action OnPlanetHold
    {
        add => _button.OnHold += value;
        remove => _button.OnHold -= value;
    }

    [SerializeField] private Image _coin;
    [SerializeField] private GameObject _progressGroup;
    [SerializeField] private TMP_Text _progressTime;
    [SerializeField] private Image _progressBar;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Image _planetIcon;
    [SerializeField] private Image _planetLock;
    [SerializeField] private SmartButton _button;
    
    public Vector2 IncomeCoinPosition => this._coin.gameObject.transform.position;
    public void ShowCoin() => _coin.enabled = true;
    public void HideCoin() => _coin.enabled = false;
    public void SetProgressText(string text) => this._progressTime.SetText(text);
    public void SetProgressFill(float value) => this._progressBar.fillAmount = value;
    public void SetPrice(string text) => this._price.SetText(text);
    public void SetIcon(Sprite icon) => this._planetIcon.sprite = icon;
    public void HideLock() => _planetLock.gameObject.SetActive(false);
    public void HideProgressBar() => _progressGroup.SetActive(false);
    public void ShowProgressBar() => _progressGroup.SetActive(true);
}
