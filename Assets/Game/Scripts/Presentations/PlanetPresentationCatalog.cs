using System.Collections.Generic;
using Modules.Planets;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPresentationCatalog
    {
        public IReadOnlyDictionary<string, PlanetPresentation> Presenters => _planetPresenters;
        private readonly Dictionary<string, PlanetPresentation> _planetPresenters = new();
        private readonly IInstantiator _instantiator;

        public PlanetPresentationCatalog(List<Planet> planets, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            
            foreach (var planet in planets)
            {
                var presenter = _instantiator.Instantiate<PlanetPresentation>(new object[] { planet });
                presenter.Initialize();
                _planetPresenters.Add(planet.Name, presenter);
            }
        }
    }
}