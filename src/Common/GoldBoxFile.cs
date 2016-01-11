using System.Collections.Generic;
using System.IO;

namespace GoldBoxExplorer.Lib
{
    public abstract class GoldBoxFile
    {
        // TODO We shouldn't expose this here
        // this is only used in textviewer and hexviewer
        public abstract IList<byte> GetBytes();


    }
}