namespace RealtyApp.Models
{
    public partial class Owner
    {
        public override string ToString()
        {
            return $"{FullName} ({PhoneNumber})";
        }
    }
}
