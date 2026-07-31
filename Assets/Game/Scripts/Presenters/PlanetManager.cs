using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Planets;
using Modules.UI;

namespace Game.Presenters
{
    public class PlanetManager : IDisposable 
    {
        private readonly Dictionary<string, Planet> _planets = new();
        private readonly Dictionary<string, PlanetPresenter> _planetPresenters;
        private readonly ParticleAnimator _coinAnimator;
        private readonly MoneyPresenter _moneyPresenter;

        public PlanetManager(List<Planet> planets,
            List<PlanetPresenter> planetPresenters,
            ParticleAnimator coinAnimator,
            MoneyPresenter moneyPresenter)
        {
            _planetPresenters = planetPresenters.ToDictionary(p => p.Name);
            _coinAnimator = coinAnimator;
            _moneyPresenter = moneyPresenter;
            foreach (var planet in planets)
            {
                _planets.Add(planet.Name, planet);
            }

            foreach (var (name, presenter) in _planetPresenters)
            {
                if (!_planets.TryGetValue(name, out Planet planet))
                    throw new System.Exception("Planet not found: " + name);

                presenter.Initialize(planet);
                presenter.OnIncomeGathered += this.AnimateIncome;
            }
        }

        public void Dispose() => UnsubscribePresenters();

        private void UnsubscribePresenters()
        {
            foreach (var presenter in _planetPresenters)
                presenter.Value.OnIncomeGathered -= this.AnimateIncome;
        }

        private void AnimateIncome(string name, int count)
        {
            if (count <= 0)
                return;
            
            _coinAnimator.Emit(_planetPresenters[name].IncomeCoinPosition,
                    _moneyPresenter.IncomeCoinPosition);
        }
    }
}