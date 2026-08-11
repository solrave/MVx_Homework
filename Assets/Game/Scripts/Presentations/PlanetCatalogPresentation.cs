using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Planets;
using Zenject;

namespace Game.Presenters
{
    public class PlanetCatalogPresentation
    {
        private readonly Dictionary<string, PlanetPresentation> _planetPresenters = new();
        private readonly Dictionary<string, IPlanetView> _planetViews;
        private readonly IInstantiator _instantiator;

        public PlanetCatalogPresentation(List<Planet> planets,
            List<IPlanetView>  planetViews,
            IInstantiator instantiator)
        {
            _planetViews = planetViews.ToDictionary(x => x.Name);
            _instantiator = instantiator;
            
            foreach (var planet in planets)
            {
                var presenter = _instantiator.Instantiate<PlanetPresentation>(new object[] { planet });
                presenter.Initialize();
                _planetPresenters.Add(planet.Name, presenter);
            }

            foreach (var (name, view) in _planetViews)
            {
                if (!_planetPresenters.TryGetValue(name, out PlanetPresentation presenter))
                    throw new Exception("Planet not found: " + name);

                view.Initialize(presenter);
            }
        }
    }
}