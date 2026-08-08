using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class PlanetCollectionPresenter : MonoBehaviour
    {
        [SerializeField]
        private PlanetCatalog _planetCatalog;
        
        [Inject]
        public void Construct(List<Planet> planets,
            List<PlanetPresenter> planetPresenters)
        {
           if (_planetCatalog.Count <= 0 || _planetCatalog is null)
               throw new Exception("Planet Catalog is missing: " + nameof(_planetCatalog));

           foreach (var planetPresenter in planetPresenters)
           {
               foreach (var planet in planets.Where(planet => planet.Name == planetPresenter.Name))
               {
                   planetPresenter.Initialize(planet);
               }
           }
        }
    }
}