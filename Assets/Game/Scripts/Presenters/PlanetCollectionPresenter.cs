using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Planets;
using Modules.UI;
using UnityEngine;

namespace Game.Presenters
{
    public class PlanetCollectionPresenter : MonoBehaviour
    {
        [SerializeField]
        private PlanetCatalog _planetCatalog;
        
        // private readonly Dictionary<string, Planet> _planets = new();
        // private readonly Dictionary<string, PlanetPresenter> _planetPresenters;
        // private readonly MoneyPresenter _moneyPresenter;

        public PlanetCollectionPresenter(List<Planet> planets,
            List<PlanetPresenter> planetPresenters,
            MoneyPresenter moneyPresenter)
        {
            //_planetPresenters = planetPresenters.ToDictionary(p => p.Name);
           // _moneyPresenter = moneyPresenter;
           if (_planetCatalog.Count <= 0 || _planetCatalog is null)
               return;
           
            foreach (var planet in _planetCatalog)
            {
                _planets.Add(planet.Name, planet);
            }

            foreach (var (name, presenter) in _planetPresenters)
            {
                if (!_planets.TryGetValue(name, out Planet planet))
                    throw new Exception("Planet not found: " + name);

                presenter.Initialize(planet);
            }
        }
    }
}