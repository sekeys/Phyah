

namespace Phyah.Reflection
{
    using Phyah.Extensions;
    public class Assembly
    {
        public static string RootPath;
        public static System.Reflection.Assembly Load(string assembly)
        {
            return System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(assembly);
        }

        public static System.Type Type(string assembly, string type)
        {
            var ass = Load(assembly);
            if (ass == null)
            {
                return null;
            }
            return ass.GetType(type);
        }
        public static System.Type Type(string assembly)
        {
            var types = assembly.Split(';');

            return Type(RootPath.CombinePath(types[0]), types[1]);
        }
    }
}
