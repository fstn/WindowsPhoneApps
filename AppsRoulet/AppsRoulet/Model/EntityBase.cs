using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace AppsRoulet.Model
{
    public interface EntityBase
    {
          void OnSaving(ChangeAction changeAction);

          void OnSaved();
    }
}