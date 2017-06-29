

namespace Phyah.EventPipeline.Selectors
{
    using System;
    using Phyah.EventPipeline.Messages;
    using Phyah.EventPipeline.Matchers;
    using System.Collections.Generic;
    using System.Reflection;
    using Phyah.EventPipeline.Attribute;
    using Phyah.TypeContainer;

    public class TypeofSelector : ISelector
    {
        public int Priority => 998;

        public string Verbs { get; set; }
        public Protocols Protocol { get; set; }

        public IEnumerable<IMatcher> Select1(Message message)
        {
            //var ls = new List<IMatcher>();
            //var messageType = message.GetType();
            //var attr = messageType.GetTypeInfo().GetCustomAttribute<EventPipeline.Attribute.TypeofMatcherAttribute>() as TypeofMatcherAttribute;
            //Type attrType = null;
            //if(attr != null)
            //{
            //    attrType = attr.MatcherType;
            //}
            //ls.Add(new TypeofMatcher(messageType, attrType));

            //var constructorAttrs = messageType.GetTypeInfo().GetCustomAttributes<EventPipeline.Attribute.MatcherConstructorAttribute>() as IEnumerable< MatcherConstructorAttribute>;            
            //if (constructorAttrs != null)
            //{
            //    foreach(var item in constructorAttrs)
            //    {
            //        ls.Add(item.Construct(message));
            //    }
            //}
            //ls.Add(new TypeofMatcher(messageType, attrType));
            return null;
            //return ls;
        }

        public IEventPipeline Select(Message message)
        {
            var messageType = message.GetType();
            var attr = messageType.GetTypeInfo().GetCustomAttribute<EventPipeline.Attribute.TypeofPipelineAttribute>() as TypeofPipelineAttribute;
            Type attrType = null;
            if (attr != null)
            {
                //attrType = attr.PipelineType;
                return Constructor.Construct(attrType) as IEventPipeline;
            }
            var constructorAttrs= messageType.GetTypeInfo().GetCustomAttributes<EventPipeline.Attribute.PipelineConstructorAttribute>() as PipelineConstructorAttribute;
            if (constructorAttrs != null)
            {
                return constructorAttrs.Construct(message);
            }
            return null;
        }
    }
}
