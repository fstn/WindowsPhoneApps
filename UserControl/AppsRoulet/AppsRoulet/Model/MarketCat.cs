using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace AppsRoulet.Model
{
    public partial class MarketCat
    {
         public  string toString()
        {
            String retour="category: TItle:"+Title+", Id:"+Id;
            return retour;
        }
    }
    [Table]
    public partial class MarketCat : EntityBase
    {
        [Column(
            IsPrimaryKey = false,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public int IdGen { get; set; }
        [Column(IsPrimaryKey = false, CanBeNull = false, IsDbGenerated = false)]
        public String Id { get; set; }
        [Column()]
        public String Title { get; set; }
        [Column()]
        public String ParentId { get; set; }
        public void OnSaving(ChangeAction changeAction) { }
        public void OnSaved () { }
    }
}
