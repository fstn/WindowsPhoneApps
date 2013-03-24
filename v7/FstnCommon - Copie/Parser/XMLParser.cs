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
        public event ParserErrorEventHandler Error;
        public virtual void parse(XDocument doc) { }
        protected void OnComplete(Object retour)
        {

            if (Completed != null)
            {
                Completed(this, retour);
            }
        }

        protected void OnError(Object retour)
        {

            if (Error != null)
            {
                Error(this, retour);
            }
        }

        public XMLParser()
        {
        }
    }
}
