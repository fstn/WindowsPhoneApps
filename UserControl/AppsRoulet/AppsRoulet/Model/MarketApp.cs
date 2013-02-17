
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using FstnDataBase.Tools;

namespace AppsRoulet.Model
{
    public partial class MarketApp
    {
        public List<String> ClientTypes { get; set; }

        //use to parse XML
        public List<MarketOffer> OffersList {
            set
            {
                Offers = new EntitySet<MarketOffer>();
                Offers.AddRange(value);
            }
        }

        //use to parse XML
        public List<MarketCat> CategoriesList
        {
            set
            {
                Categories = new EntitySet<MarketCat>();
                Categories.AddRange(value);
            }
        }

        public MarketApp()
        {
        }

        public string toString()
        {
            String retour = "application: Title:" + Title + ", Id:" + Id;
           /* if (Categories != null)
            {
                foreach (MarketCat cat in Categories)
                {
                    retour += "\n \t" + cat.ToString();
                }
            }
            if (Offers != null)
            {
                foreach (MarketOffer offer in Offers)
                {
                    retour += "\n \t" + offer.ToString();
                }
            }*/
            retour += "\n " + Image.toString();
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
            IsPrimaryKey = false,
            IsDbGenerated = true,
            DbType = "INT NOT NULL",
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
        [Column()]
        public int UserRatingCount { get; set; }

        [Column(IsPrimaryKey = false)]
        internal int IdImage { get; set; }

        private EntityRef<MarketImage> _image=new EntityRef<MarketImage>();

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

        private EntitySet<MarketCat> _categories = new EntitySet<MarketCat>();
        [Association(Name = "FK_MarketCat_MarketAppCat", Storage = "_categories", OtherKey = "IdGen")]
        public EntitySet<MarketCat> Categories
        {
            get { return this._categories; }
            set
            {
                if (this._categories == null)
                    this._categories = new EntitySet<MarketCat>();
                this._categories.Assign(value);
                OnPropertyChanged("MarketCats");
            }
        }

        private EntitySet<MarketOffer> _offers = new EntitySet<MarketOffer>();
        // [Association(Name = "FK_MarketApp_MarketAppOffers", Storage = "_offers", ThisKey = "IdGen", OtherKey = "IdGen")]
        [Association(Name = "FK_MarketApp_MarketAppOffers", Storage = "_offers", ThisKey = "IdGen", OtherKey = "IdGen")]
        public EntitySet<MarketOffer> Offers
        {
            get { return this._offers; }
            set
            {
                if (this._offers == null)
                    this._offers = new EntitySet<MarketOffer>();
                this._offers.Assign(value);
                OnPropertyChanged("Offers");
            }
        }
        public void OnSaving(ChangeAction changeAction)
        {
            switch (changeAction)
            {
                case ChangeAction.Insert:
                    List<MarketCat> listOfCats = new List<MarketCat>();
                    foreach (MarketCat cat in Categories)
                    {
                        MyDataContext db = MyDataContextFactory.Instance.DataContext;
                        MarketCat tmp = db.MarketCategories.SingleOrDefault(c => c.Id == cat.Id);
                        if (tmp != null )
                        {
                            MyDataContextFactory.Instance.DataContext.MarketCategories.Attach(cat, tmp);
                            MyDataContextFactory.Instance.DataContext.SubmitChanges();
                            listOfCats.Add(tmp);
                        }
                        else
                        {
                            listOfCats.Add(cat);
                        }
                    }
                    this.CategoriesList = listOfCats;
                    break;
                case ChangeAction.Update:
                    break;
                default:
                    break;
            }
        }
        public void OnSaved()
        {
        }
    }

}
