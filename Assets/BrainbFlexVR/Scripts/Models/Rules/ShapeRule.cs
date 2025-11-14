namespace BrainFlexVR
{
    public class ShapeRule : IRule
    {
        public string Name => "Shape";
        public bool Evaluate(Card responseCard, TargetCard targetCard)
            => responseCard.Shape == targetCard.Shape;
    }
}