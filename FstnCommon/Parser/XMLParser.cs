using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FstnCommon.Parser
{
    public abstract class XMLParser:IParser
    {
        public event ParserCompleteEventHandler Completed;
        public virtual void parse(XDocument doc) { }
        protected void OnComplete(Object retour)
        {

            if (Completed != null)
            {
                Completed(this, retour);

            }
        }

        public XMLParser()
        {
        }
    }
}
