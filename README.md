# Dapper SQL builder
Test project for setting up an SQL client using Dapper.

The basic usage is:
1. Create a class for each table that inherits from 'TableBase'.
2. For each entity, create a class that inherits from 'EntityBase'.
2. Use the Update, Select, Insert, or Delete methods with an array that contains a list of conditions

The core files needed to make it work are SqlConditions, SqlDataAccess & TableBase.
##
- All values are prepared using Sql parameters unless a 'raw' flag has been provided.
- Authentication is handled using Jwt tokens & Microsoft's identity system.
##
**Note:** The repository is an example and it does not represent the full project.
