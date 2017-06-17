

namespace Phyah.Process
{
    using System;
    using System.Collections.Concurrent;
    using System.Text;
    using System.Reflection;
    using Phyah.Interface;
    using System.Threading.Tasks;

    public class ProcessorMappingFactory
    {
        public async Task<IProcessMapping> Create(IParameter parameter)
        {
            return await Task.Run<IProcessMapping>(() =>
            {
                IProcessMapping processMapping = null;
                if (parameter.Contains("debug"))
                {
                    var param = parameter.Get<IParameter>("debug");
                    if (param.Contains("processmapping"))
                    {
                        string processmapping = param.Get<string>("processmapping");
                        if (processmapping.Contains(";"))
                        {
                            var pms = processmapping.Split(';');
                            var assembly = Assembly.Load(new AssemblyName(pms[1]));
                            var type = assembly.GetType(pms[0]);
                            processMapping = Activator.CreateInstance(type) as IProcessMapping;
                        }
                        else if (Mapping.ContainsKey(processmapping))
                        {
                            Mapping.TryGetValue(processmapping, out processMapping);
                        }
                    }
                }
                else
                {
                    foreach (var item in Mapping.Values)
                    {
                        if (item.Match(parameter))
                        {
                            processMapping = item;
                            break;
                        }
                    }
                }
                new Debug.ConsoleAssert().Raw((processMapping == null).ToString());
                return processMapping == null ? new DefaultProcessMapping() : processMapping;
            });
        }

        protected ConcurrentDictionary<string, IProcessMapping> Mapping = new ConcurrentDictionary<string, IProcessMapping>();
        public void Register(IProcessMapping proccessMapping)
        {
            string name = nameof(proccessMapping);
            Mapping.TryAdd(name, proccessMapping);
        }
        public void Unregister(IProcessMapping processMapping)
        {

            string name = nameof(processMapping);
            Mapping.TryRemove(name, out processMapping);
        }
        public IProcessMapping Unregister(string name)
        {
            IProcessMapping processMapping;
            Mapping.TryRemove(name, out processMapping);
            return processMapping;
        }

        public static void RegisterFactory(ProcessorMappingFactory factory)
        {
            Factory = factory;
        }
        public static ProcessorMappingFactory Factory
        {
            get;
            private set;
        }
    }
}
