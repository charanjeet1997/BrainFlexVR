namespace BrainFlexVR
{
    public class ColorRule : IRule
    {
        public string Name => "Color";
        public bool Evaluate(Card responseCard, TargetCard targetCard)
            => responseCard.Color == targetCard.Color;
    }
}