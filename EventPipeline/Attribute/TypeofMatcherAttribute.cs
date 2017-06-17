using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventPipeline.Attribute
{
   public class TypeofMatcherAttribute:System.Attribute
    {
        public readonly Type MatcherType;
        public TypeofMatcherAttribute( Type matcherType)
        {
            this.MatcherType = matcherType;
        }
    }
}
