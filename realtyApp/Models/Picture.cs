//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealtyApp.Models
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public int RealEstateId { get; set; }
    
        public virtual RealEstate RealEstate { get; set; }
    }
}
