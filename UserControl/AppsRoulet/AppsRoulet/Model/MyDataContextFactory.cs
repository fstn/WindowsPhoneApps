using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppsRoulet.Model
{
    public class MyDataContextFactory
    {
        public MyDataContext DataContext { get; set; }
        private static MyDataContextFactory instance;
        public static MyDataContextFactory Instance { get { 
            if(instance==null)
                instance=new MyDataContextFactory();
            return instance;
        } }
        private MyDataContextFactory()
        {
            DataContext = new MyDataContext(MyDataContext.DBConnectionString);
        }

    }
}
