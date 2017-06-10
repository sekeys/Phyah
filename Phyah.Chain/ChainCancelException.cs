
namespace Phyah.Chain
{

    using System;

    public class ChainCancelException:Exception
    {
        public ChainCancelException()
        {

        }
        public ChainCancelException(string msg):base(msg)
        {

        }
    }
}