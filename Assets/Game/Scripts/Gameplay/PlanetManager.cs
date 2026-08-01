using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Planets;
using Modules.UI;
using Zenject;

namespace Game.Presenters
{
    public class PlanetManager
    {
        //private readonly Dictionary<string, Planet> _planets = new();
        private readonly Dictionary<string, PlanetPresenter> _planetPresenters = new();
        private readonly Dictionary<string, PlanetView> _planetViews;
        private readonly MoneyPresenter _moneyPresenter;
        private IInstantiator _instantiator;

        public PlanetManager(List<Planet> planets,
            List<PlanetView>  planetViews,
            MoneyPresenter moneyPresenter,
            IInstantiator instantiator)
        {
            _planetViews = planetViews.ToDictionary(x => x.Name);
            _moneyPresenter = moneyPresenter;
            _instantiator = instantiator;
            
            foreach (var planet in planets)
            {
                var presenter = _instantiator.Instantiate<PlanetPresenter>(new object[] { planet });
                presenter.Initialize();
                _planetPresenters.Add(planet.Name, presenter);
            }

            foreach (var (name, view) in _planetViews)
            {
                if (!_planetPresenters.TryGetValue(name, out PlanetPresenter presenter))
                    throw new Exception("Planet not found: " + name);

                view.Initialize(presenter);
            }
        }
    }
}