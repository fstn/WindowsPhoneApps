using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AppsRoulet.util
{
    public class BDDLogger :TextWriter
    {

        public override void Write(char[] buffer, int index, int count)
        {
            Debug.WriteLine(new String(buffer, index, count));
        }

        public override void Write(string value)
        {
            Debug.WriteLine(value);
        }

        public override Encoding Encoding
        {
            get { return null; }
        }
    }
}
