using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace AppsRoulet.Model
{
    internal class ChangeEntity
    {
        public ChangeAction ChangeAction { get; set; }

        public EntityBase Entity { get; set; }
    }
}