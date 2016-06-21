using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schema_Application.Models.Factory
{
    public class RulesFactory
    {
        public ITimesADay GetTimesADay()
        {
            return new OnlyOnePerDay();
        }
    }
}