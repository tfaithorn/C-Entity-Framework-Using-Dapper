# Dapper SQL builder
This is a test project to set up an SQL client in .net with Dapper.
The basic usage is:
1. Create a class for each  table which inherits from 'TableBase'.
2. For each entity add a class that inherits from 'EntityBase' as a generic contraint.
2. User the Update, Select, Insert, or Delete methods with a list of conditions

The core files are SqlConditions, SqlDataAccess & TableBase.
##
 - All values are prepared with Sql parameters unless a 'raw' flag is set on the SqlCondition class.
- Authentication is handled with Jwt tokens & Microsoft's identity system
##
**Note:** This repository is only an example and it does not represent the full project.
