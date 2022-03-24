namespace IpGeoInformer.Tests.PageModel
{
    public abstract class PageBase
    {
        public abstract bool IsPresent();
        
        public PageBase WaitPresent()
        {
            Waiter.Wait(IsPresent);
            return this;
        }
    }
}