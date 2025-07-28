
namespace DatabaseConnect.Controllers
{
    public class HttpPostedFileBase
    {
        public int ContentLength { get; internal set; }
        public Stream InputStream { get; internal set; }
    }
}