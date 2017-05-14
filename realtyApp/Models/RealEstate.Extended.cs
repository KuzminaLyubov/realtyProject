namespace RealtyApp.Models
{
    public partial class RealEstate
    {
        public override string ToString()
        {
            return $"{Title} ({Address})";
        }
    }
}
