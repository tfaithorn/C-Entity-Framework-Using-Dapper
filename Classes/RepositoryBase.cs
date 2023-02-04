namespace Custom_C_Sharp_Entity_Framework.Classes;

public abstract class RepositoryBase
{
    protected SqlDataAccess db = SqlDataAccess.GetInstance();
}
