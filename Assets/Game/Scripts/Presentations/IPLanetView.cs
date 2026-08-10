namespace Game.Presenters
{
    public interface IPlanetView
    {
        public string Name { get; }
        public void Initialize(PlanetPresentation planetPresentation);
    }
}