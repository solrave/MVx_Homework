using System.Collections.Generic;
using Game.Presenters;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class ViewCollection : IInitializable
    {
        private readonly List<PlanetView> _planetViews;
        private PresentationCollection _presentationCollection;
        
        public ViewCollection(List<PlanetView> planetViews,
                                PresentationCollection collection)
        {
            _planetViews = planetViews;
            _presentationCollection = collection;
        }

        public void Initialize()
        {
            foreach (var planetView in _planetViews)
            {
                if (_presentationCollection.Presentations
                    .TryGetValue(planetView.Id, out var presentation))
                    planetView.Initialize(presentation);
                else
                    Debug.LogWarning($"No presenter for: {planetView.Id}");
            }
        }
    }
}