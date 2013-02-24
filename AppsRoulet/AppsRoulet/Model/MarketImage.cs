using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using FstnDataBase.Tools;

namespace AppsRoulet.Model
{
    public partial class MarketImage
    {
        public string toString()
        {
            String retour = "image: Id:" + Id;
            return retour;
        }
    }

    [Table]
    public partial class MarketImage : BindableObject, EntityBase
    {
        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public int IdGen { get; set; }
        [Column(IsPrimaryKey = false, CanBeNull = false, IsDbGenerated = false)]
        public String Id { get; set; }
        public void OnSaving(ChangeAction changeAction) { }
        public void OnSaved (){ }
    }
}
