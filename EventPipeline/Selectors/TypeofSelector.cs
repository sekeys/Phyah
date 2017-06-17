

namespace Phyah.EventPipeline.Selectors
{
    using System;
    using Phyah.EventPipeline.Messages;
    using Phyah.EventPipeline.Matchers;
    using System.Collections.Generic;
    using System.Reflection;
    using Phyah.EventPipeline.Attribute;

    public class TypeofSelector : ISelector
    {
        public int Priority => 999;

        public IEnumerable<IMatcher> Select(Message message)
        {
            var ls = new List<IMatcher>();
            var messageType = message.GetType();
            var attr = messageType.GetTypeInfo().GetCustomAttribute<EventPipeline.Attribute.TypeofMatcherAttribute>() as TypeofMatcherAttribute;
            Type attrType = null;
            if(attr != null)
            {
                attrType = attr.MatcherType;
            }
            ls.Add(new TypeofMatcher(messageType, attrType));

            var constructorAttrs = messageType.GetTypeInfo().GetCustomAttributes<EventPipeline.Attribute.MatcherConstructorAttribute>() as IEnumerable< MatcherConstructorAttribute>;            
            if (constructorAttrs != null)
            {
                foreach(var item in constructorAttrs)
                {
                    ls.Add(item.Construct(message));
                }
            }
            ls.Add(new TypeofMatcher(messageType, attrType));
            return ls;
        }
    }
}
