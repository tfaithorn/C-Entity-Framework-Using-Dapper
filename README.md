# C-Entity-Framework-Using-Dapper
This is an example of how to set up an entity Framework using Dapper.
The basic usage is:
1. Create a class that represents a table, which inherits from 'TableBase'.
Add a class that inherits from 'EntityBase' as a generic contraint.
2. Access the database with the Update, Select, Insert, or Delete methods. 
##
All values are prepared with Sql parameters unless a 'raw' flag is set on the SqlCondition class.
##
Authentication is handled using Microsoft's Identity Service & Jwt tokens
##
**Note:** This repository is only an example and it does not represent a finished project.
