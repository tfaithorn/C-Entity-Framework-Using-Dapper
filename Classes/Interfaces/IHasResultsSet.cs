namespace Custom_C_Sharp_Entity_Framework.Classes.Interfaces
{
    public interface IHasResultsSet<T1, T2>
       where T1 : OutputBase
       where T2 : OutputParameters
    {
        public ResultsSet<T1> GetResultsSet(
            List<SqlCondition> conditions = null,
            T2 outputParameters = null);
    }
}
