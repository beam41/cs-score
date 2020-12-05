using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsScore.Enums;

namespace CsScore.Attribute
{
    public class AccessLevelAttribute : System.Attribute
    {
        public AccessLevel Level { get; set; }

        public AccessLevelAttribute(AccessLevel level = AccessLevel.None)
        {
            Level = level;
        }
    }
}
