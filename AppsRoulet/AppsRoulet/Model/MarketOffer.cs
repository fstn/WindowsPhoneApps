using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace AppsRoulet.Model
{

    public partial class MarketOffer
    {
        public  string toString()
        {
            String retour = "offer: Id:" + Id+" Price: "+Price ;
            return retour;
        }
    }
    [Table]
    public partial class MarketOffer : EntityBase
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
        [Column()]
        public String MediaInstanceId { get; set; }
        [Column()]
        public double Price { get; set; }
        [Column()]
        public String PriceCurrencyCode { get; set; }
        public void OnSaving(ChangeAction changeAction) { }
        public void OnSaved() { }
    }

}
