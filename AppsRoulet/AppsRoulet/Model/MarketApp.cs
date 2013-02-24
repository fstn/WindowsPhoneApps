
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FstnDataBase.Tools;

namespace AppsRoulet.Model
{
    public partial class MarketApp
    {
        public List<String> ClientTypes { get; set; }

        public MarketApp()
        {
        }

        public string toString()
        {
            String retour = "application: Title:" + Title + ", Id:" + Id;
      
            retour += "\n " + Image;
            return retour;
        }

    }
        [Table]
    public partial class MarketApp : BindableObject ,EntityBase
    {
        [Column()]
        public String Title { get; set; }
        [Column()]
        public DateTime Update { get; set; }

        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL IDENTITY",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public int IdGen { get; set; }

        [Column(IsPrimaryKey = false, CanBeNull=false,IsDbGenerated=false)]
        public String Id;

        [Column(IsPrimaryKey = false, DbType = "NVARCHAR(50)  NULL ")]
        public String SortTitle { get; set; }
        [Column()]
        public DateTime ReleaseDate { get; set; }
        [Column()]
        public String Version { get; set; }
        [Column()]
        public double AverageUserRating { get; set; }
        internal String AverageUserRatingString

        {
            set
            {
                try
                {
                    this.AverageUserRating = Convert.ToDouble(value);
                }
                catch (Exception e)
                {
                    Debugger.Log(1, "error", "can't parse " + value + " to double");
                }
            }
        }
        [Column()]
        public int UserRatingCount { get; set; }
        internal String UserRatingCountString
        {
            set
            {
                try
                {
                    this.UserRatingCount= Convert.ToInt16(value);
                }
                catch (Exception e)
                {
                    Debugger.Log(1, "error", "can't parse " + value + " to double");
                }
            }
        }
            
        [Column(IsPrimaryKey = false)]
        public String Image { get; set; }

        [Column(IsPrimaryKey = false)]
        internal String Categorie { get; set; }
        [Column(IsPrimaryKey = false)]
        internal Double Price { get; set; }

        internal String PriceString
        {
            set
            {
                try
                {
                    this.Price = Convert.ToDouble(value);
                }
                catch (Exception e)
                {
                    Debugger.Log(1,"error","can't parse "+value+" to double");
                }
            }
        }
     
        public void OnSaving(ChangeAction changeAction)
        {
        }
        public void OnSaved()
        {
        }
    }

}
