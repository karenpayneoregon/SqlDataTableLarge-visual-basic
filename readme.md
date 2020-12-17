# About

Simple no frills code sample to demonstrate exporting large DataTable to a comma delimited text file.

The focus in the code sample is Partitioning DataRows, see notes below.

### Requires

- To run this code sample **as is** [Microsoft Visual Studio 2019](https://visualstudio.microsoft.com/vs/) is required. If using for instance Visual Studio 2017 simply copy the files into a new Visual Studio solution and build/run.
- Microsoft SQL-Server (at least [Express edition](https://www.microsoft.com/en-us/download/details.aspx?id=55994))
- Plus [Microsoft SQL-Server Management Studio](https://www.microsoft.com/en-us/download/details.aspx?id=55994) (for running the script)
- Run the file script.sql to create the database, table along with populating the table.
- if the build fails run NuGet Restore packages.

### Notes

- No assertion performed on data, each DataRow is exported by String.Join on ItemArray of each row.
- Partitioning is done using a language extension.
- Partition is done [asynchronously](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/) with an option to cancel
- File name is hard-coded.