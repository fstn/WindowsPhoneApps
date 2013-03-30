namespace GiveMeSomething.Resources
{
    public class ResourceWrapper
    {
        private static Msg localizedResources = new Msg();

        public static  Msg ResourceGetter
        {
            get { return localizedResources; }
        }
    }    
}
