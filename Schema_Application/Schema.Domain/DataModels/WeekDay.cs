//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Schema.Domain.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class WeekDay
    {
        public WeekDay()
        {
            this.ActivitySummeries = new HashSet<ActivitySummery>();
        }
    
        public int WeekDayId { get; set; }
        public string Day { get; set; }
    
        public virtual ICollection<ActivitySummery> ActivitySummeries { get; set; }
    }
}
