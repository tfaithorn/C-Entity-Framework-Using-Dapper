using System.Diagnostics.Contracts;

namespace Custom_C_Sharp_Entity_Framework.Classes.User
{
    public class UserOutputParameters : OutputParameters
    {
        public Boolean includeCount = false;
        public int? numberOfRows;
        public int? page = null;
        public List<Guid> yearGuids = null;
    }
}
