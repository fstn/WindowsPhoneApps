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

        public override void SubmitChanges(ConflictMode failureMode)
        {
            // Get the entities that are to be inserted / updated / deleted
            ChangeSet changeSet = GetChangeSet();

            // Get a single list of all the entities in the change set
            IEnumerable<object> changeSetEntities = changeSet.Deletes;
            changeSetEntities = changeSetEntities.Union(changeSet.Inserts);
            changeSetEntities = changeSetEntities.Union(changeSet.Updates);

            // Get a single list of all the enitities that inherit from EntityBase
            IEnumerable<ChangeEntity> entities =
                 from entity in changeSetEntities.Cast<EntityBase>()
                 select new ChangeEntity()
                 {
                     ChangeAction =
                          changeSet.Deletes.Contains(entity) ? ChangeAction.Delete
                        : changeSet.Inserts.Contains(entity) ? ChangeAction.Insert
                        : changeSet.Updates.Contains(entity) ? ChangeAction.Update
                        : ChangeAction.None,
                     Entity = entity as EntityBase
                 };

            // "Raise" the OnSaving event for the entities 
            foreach (ChangeEntity entity in entities)
            {
                entity.Entity.OnSaving(entity.ChangeAction);
            }

            // Save the changes
            base.SubmitChanges(failureMode);

            // "Raise" the OnSaved event for the entities
            foreach (ChangeEntity entity in entities)
            {
                entity.Entity.OnSaved();
            }
        }

        public Table<MarketApp> MarketApps
        {
            get
            {
                return this.GetTable<MarketApp>();
            }
        }
        public Table<MarketCat> MarketCategories
        {
            get
            {
                return this.GetTable<MarketCat>();
            }
        }
    }/*
        public Table<MarketImage> MarketImages
        {
            get
            {
                return this.GetTable<MarketImage>();
            }
        }
        public Table<LinkCategoryToApp> LinkCategoryToApps
        {
            get
            {
                return this.GetTable<LinkCategoryToApp>();
            }
        }
        public Table<MarketOffer> MarketOffers
        {
            get
            {
                return this.GetTable<MarketOffer>();
            }
        }
    }
    
    [Table]
    public partial class LinkOfferToApp : BindableObject, EntityBase
    {
        [Column(IsPrimaryKey = true, Name = "MarketApp")]
        private int marketAppId;
        private EntityRef<MarketApp> _app = new EntityRef<MarketApp>();
        [Association(Name = "FK_LinkOfferToApp_MarketApps", IsForeignKey = true,
               Storage = "_app ", ThisKey = "marketAppId")]
        public MarketApp MarketApp
        {
            get { return _app.Entity; }
            set { _app.Entity = value; }
        }

       
        [Column(IsPrimaryKey = true, Name = "MarketOffer")]
        private int marketOfferId;
        private EntityRef<MarketOffer> _offer = new EntityRef<MarketOffer>();
        [Association(Name = "FK_LinkOfferToApp_MarketOffers", IsForeignKey = true,
             Storage = "_offer", ThisKey = "marketOfferId")]
        public MarketOffer MarketOffer
        {
            get { return _offer.Entity; }
            set { _offer.Entity = value; }
        }
        public void OnSaving(ChangeAction changeAction) { }
        public void OnSaved () { }
    }

    [Table]
    public partial class LinkCategoryToApp : BindableObject, EntityBase
    {
        [Column(IsPrimaryKey = true, Name = "MarketApp")]
        private int marketAppId;
        private EntityRef<MarketApp> _app4App = new EntityRef<MarketApp>();
        [Association(Name = "FK_LinkCategoryToApp_MarkeApp", IsForeignKey = true,
               Storage = "_app4App", ThisKey = "marketAppId")]
        public MarketApp MarketApp
        {
            get { return _app4App.Entity; }
            set { _app4App.Entity = value; }
        }
       

        [Column(IsPrimaryKey = true, Name = "MarketCat")]
        private int marketCatId;
        private EntityRef<MarketCat> _cat = new EntityRef<MarketCat>();
        [Association(Name = "FK_LinkCategoryToApp_MarketCat", IsForeignKey = true,
             Storage = "_cat", ThisKey = "marketCatId")]
        public MarketCat MarketCat
        {
            get { return _cat.Entity; }
            set { _cat.Entity = value; }
        }
        public void OnSaving(ChangeAction changeAction) { }
        public void OnSaved (){ }
   } 

    [Table]
    public partial class LinkTypesToApp : BindableObject, EntityBase
    {
        [Column(IsPrimaryKey = true, Name = "MarketApp")]
        private int marketAppId;
        private EntityRef<MarketApp> _app = new EntityRef<MarketApp>();
        [Association(Name = "FK_LinkTypesToApp_MarketApp", IsForeignKey = true,
               Storage = "_app", ThisKey = "marketAppId")]
        public MarketApp MarketApp
        {
            get { return _app.Entity; }
            set { _app.Entity = value; }
        }

        [Column(IsPrimaryKey = true, Name = "paymentType")]
        private String PaymentType;
        public void OnSaving(ChangeAction changeAction) { }
        public void OnSaved () { }
    }   */
}