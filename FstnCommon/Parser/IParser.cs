using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FstnCommon.Parser
{
    public delegate void ParserCompleteEventHandler(object sender, Object obj);
    public interface IParser
    {
        void parse(XDocument doc);
    }
}
