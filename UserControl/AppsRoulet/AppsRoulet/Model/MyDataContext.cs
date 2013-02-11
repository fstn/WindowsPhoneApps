using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using FstnDataBase.Tools;

namespace AppsRoulet.Model
{
    public class MyDataContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/BDD.sdf";

        public MyDataContext(string connectionString)
            : base(connectionString)
        {
        }

        public Table<MarketApp> MarketApps
        {
            get
            {
                return this.GetTable<MarketApp>();
            }
        }
        public Table<MarketImage> Images
        {
            get
            {
                return this.GetTable<MarketImage>();
            }
        }
    }

    [Table]
    public partial class MarketApp : BindableObject
    {
        //http://www.w3.org/2005/Atom
        [Column()]
        public String Title { get; set; }
        [Column()]
        public DateTime Update { get; set; }

        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public int IdGen { get; set; }

        [Column(IsPrimaryKey = false, DbType = "NVARCHAR(50)  NULL")]
        public String Id;

        //http://schemas.zune.net/catalog/apps/2008/02
        [Column(IsPrimaryKey = false, DbType = "NVARCHAR(50)  NULL")]
        public String SortTitle { get; set; }
        [Column()]
        public DateTime ReleaseDate { get; set; }
        [Column()]
        public String Version { get; set; }
        [Column()]
        public double AverageUserRating { get; set; } 
        [Column()]
        public int UserRatingCount { get; set; }

        [Column(IsPrimaryKey = false, DbType = "INT NOT NULL")]
        internal int IdImage { get; set; }

        private EntityRef<MarketImage> _image;
        [Association(Storage = "_image", ThisKey = "IdImage", OtherKey = "IdGen", IsForeignKey = true)]
        public MarketImage Image
        {
            get { return this._image.Entity; }
            set
            {
                if (this._image.Entity != value)
                {
                    this._image.Entity = value;
                    if (value != null) IdImage = value.IdGen;
                    OnPropertyChanged("Image");
                }
            }
        }
    }
    [Table]
    public partial class MarketImage : BindableObject
    {
        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public int IdGen { get; set; }
        [Column()]
        public String Id { get; set; }
    }
}