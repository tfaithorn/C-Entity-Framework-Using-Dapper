namespace Custom_C_Sharp_Entity_Framework.Classes.User
{
    public class UserTable : TableBase<UserEntity>
    {
        private static readonly string tableName = "users";

        public static string GetTableNameStatic()
        {
            return tableName;
        }

        public override List<SqlField> GetFields()
        {
            return new List<SqlField>() 
            {
                new SqlField("id", tableName),
                new SqlField("guid", tableName),
                new SqlField("firstName", tableName),
                new SqlField("lastName", tableName),
                new SqlField("username", tableName),
                new SqlField("status", tableName),
                new SqlField("email", tableName),
                new SqlField("middleName", tableName),
                new SqlField("createdAt", tableName)
            };
        }

        public List<SqlField> GetOutputFields()
        {
            return new List<SqlField>()
            {
                new SqlField("guid", UserTable.tableName),
                new SqlField("firstName", UserTable.tableName),
                new SqlField("lastName", UserTable.tableName),
                new SqlField("username", UserTable.tableName),
                new SqlField("status", UserTable.tableName),
                new SqlField("email", UserTable.tableName),
                new SqlField("middleName", UserTable.tableName)
            };
        }

        public override string GetTableName()
        {
            return tableName;
        }
    }
}
