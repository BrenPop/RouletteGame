using DerivcoAssessment.Enums;

namespace DerivcoAssessment.Models
{
    public class Spin : BaseEntity
    {
        public int Number { get; set; }
        public BetColour Colour { get; set; }
    }
}
