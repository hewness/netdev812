//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Addresses
{
    using System;
    using System.Collections.Generic;
    
    public partial class Phone
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneType { get; set; }
    
        public virtual Entry Entry { get; set; }
    }
}