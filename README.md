[![Build status](https://ci.appveyor.com/api/projects/status/e732n85eeesdasy2?svg=true)](https://ci.appveyor.com/project/bilal-fazlani/exceltosqlscripts)

# Setup instructions

This small command line tool to help you convert data of excel files into insert statements in SQL syntax.

1. Navigate to [dotnet core website](https://www.microsoft.com/net/core) and follow instructions to install dotnet core
2. `dotnet tool install -g ExcelToSQLScripts.Console`
3. `excel2sql --help`

Note: Ensure that you have `~/.dotnet/tools` path added in your $PATH variable


# Generating SQL scripts

You can generate 3 types of scripts i.e. insert scipts, update scripts & merge scripts.

##### Sample Input

![Excel](/Readme/Sample_Input.png "Excel")

### Insert scripts

```
excel2sql insert -i <PATH_TO_XLSX_FILE> -o <OUTPUT_DIRECTORY_FOR_SQL_FILES>
```

##### Sample Output

```sql
INSERT INTO EMPLOYEES (ID, NAME, LOCATION) VALUES (1, 'John', 'India');
INSERT INTO EMPLOYEES (ID, NAME, LOCATION) VALUES (2, 'Jason', 'US');
```

### Update scripts

```
excel2sql update -i <PATH_TO_XLSX_FILE> -o <OUTPUT_DIRECTORY_FOR_SQL_FILES>
```

##### Sample Output

```sql
UPDATE EMPLOYEES SET NAME = 'John', LOCATION = 'India' WHERE ID = 1;
UPDATE EMPLOYEES SET NAME = 'Jason', LOCATION = 'US' WHERE ID = 2;
```

### Merge scripts

```
excel2sql merge -i <PATH_TO_XLSX_FILE> -o <OUTPUT_DIRECTORY_FOR_SQL_FILES>
```

##### Sample Output

```sql
MERGE INTO EMPLOYEES T
USING (SELECT 1 ID, 'John' NAME, 'India' LOCATION FROM DUAL) D
ON (T.ID = D.ID)
WHEN MATCHED THEN 
UPDATE SET T.NAME = D.NAME, T.LOCATION = D.LOCATION
WHEN NOT MATCHED THEN 
INSERT (ID, NAME, LOCATION) VALUES (D.ID, D.NAME, D.LOCATION);

MERGE INTO EMPLOYEES T
USING (SELECT 2 ID, 'Jason' NAME, 'US' LOCATION FROM DUAL) D
ON (T.ID = D.ID)
WHEN MATCHED THEN 
UPDATE SET T.NAME = D.NAME, T.LOCATION = D.LOCATION
WHEN NOT MATCHED THEN 
INSERT (ID, NAME, LOCATION) VALUES (D.ID, D.NAME, D.LOCATION);
```
