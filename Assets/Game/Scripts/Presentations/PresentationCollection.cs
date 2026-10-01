using System.Collections.Generic;
using Modules.Planets;
using Zenject;

namespace Game.Presenters
{
    public class PresentationCollection
    {
        public IReadOnlyDictionary<string, PlanetPresentation> Presentations => _planetPresentations;
        private readonly Dictionary<string, PlanetPresentation> _planetPresentations = new();
        private readonly PlanetPresentation.Factory _factory;

        public PresentationCollection(List<Planet> planets, PlanetPresentation.Factory factory)
        {
            _factory = factory;

            foreach (var planet in planets)
            {
                var presentation = _factory.Create(planet);
                presentation.Initialize();
                _planetPresentations.Add(planet.Name, presentation);
            }
        }
    }
}