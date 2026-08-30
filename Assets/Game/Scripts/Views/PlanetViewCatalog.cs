using System.Collections.Generic;
using System.Linq;
using Game.Presenters;
using Zenject;

namespace Game.Views
{
    public class PlanetViewCatalog : IInitializable
    {
        private readonly List<PlanetView> _planetViews;
        private readonly PlanetPresentationCatalog _presentationCatalog;

        public PlanetViewCatalog(List<PlanetView> planetViews,
                                 PlanetPresentationCatalog catalog)
        {
            _planetViews = planetViews;
            _presentationCatalog = catalog;
        }

        public void Initialize()
        {
            var presenters = _presentationCatalog.Presenters.Values.ToList();
            for (int i = 0; i < _planetViews.Count; i++)
                _planetViews[i].Initialize(presenters[i]);
        }
    }
}