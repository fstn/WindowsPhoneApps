using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FstnUserControl.Resources;

namespace FstnUserControl.Error
{
    public delegate void WantToNavigateEventHandler(Uri uri);
    public class ErrorService
    {
        public event WantToNavigateEventHandler WantToNavigate;
        private static ErrorService instance=new ErrorService();
        public IErrorDisplayer ErrorDisplayer;
        public static ErrorService Instance { get { return instance; } }
        private ErrorService()
        {

        }
        public void AddError(object sender,String error,ErrorType type, Exception e=null){
            Uri uri = null;
            if (type == ErrorType.NETWORKING_PROBLEM)
            {
                ErrorDisplayer.Show(FstnUserControlMsg.NoConnection);
                // uri = new System.Uri("/FstnDesign;component/Error.xaml?error=no results", System.UriKind.Relative);
            }
            if (type == ErrorType.EMPTY_RESPONSE_FROM_SERVER)
            {
                ErrorDisplayer.Show(FstnUserControlMsg.NoConnection);
               // uri = new System.Uri("/FstnDesign;component/Error.xaml?error=no results", System.UriKind.Relative);
            }
            
            if (type == ErrorType.CANT_CAST_ORIENTED)
            {
                ErrorDisplayer.Show("can't cast to OrientedCameraImage you need to use OrientedCameraImage");
               // uri = new System.Uri("/FstnDesign;component/Error.xaml?error=no results", System.UriKind.Relative);
            }
            
            if (WantToNavigate != null && uri!=null)
            {
                WantToNavigate(uri);

            }
        }
    }
}
