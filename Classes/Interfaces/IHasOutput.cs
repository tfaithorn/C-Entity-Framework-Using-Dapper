namespace Custom_C_Sharp_Entity_Framework.Classes.Interfaces
{
    public interface IHasOutput<T1, T2> 
        where T1 : OutputBase
        where T2 : OutputParameters
    {
        public List<T1> GetOutput(
            List<SqlCondition> conditions = null, 
            T2 outputParameters = null);
    }
}
