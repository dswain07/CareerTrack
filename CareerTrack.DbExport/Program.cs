
using Microsoft.Data.Sqlite;
using System.Text;

// Locate the existing SQLite database.
// Run this utility from the solution root directory.
string solutionDirectory = Directory.GetCurrentDirectory();

string databasePath = Path.Combine(
    solutionDirectory,
    "CareerTrack",
    "careertrack.db"
);

string outputPath = Path.Combine(
    solutionDirectory,
    "CareerTrack",
    "Scripts",
    "DML.sql"
);

if (!File.Exists(databasePath))
{
    Console.WriteLine($"Database not found: {databasePath}");
    return;
}

// Open the database in read-only mode.
var connectionString = new SqliteConnectionStringBuilder
{
    DataSource = databasePath,
    Mode = SqliteOpenMode.ReadOnly
}.ToString();

using var connection = new SqliteConnection(connectionString);
connection.Open();

// Export parent tables before dependent tables.
string[] tables =
{
    "Companies",
    "ApplicationStatuses",
    "Contacts",
    "JobApplications",
    "Interviews"
};

var sql = new StringBuilder();

sql.AppendLine("-- CareerTrack DML Script");
sql.AppendLine("-- Generated from the existing SQLite database");
sql.AppendLine("-- Contains sample records for all five application tables");
sql.AppendLine();
sql.AppendLine("BEGIN TRANSACTION;");
sql.AppendLine();

foreach (string table in tables)
{
    Console.WriteLine($"Exporting {table}...");

    using var command = connection.CreateCommand();
    command.CommandText = $"SELECT * FROM \"{table}\";";

    using var reader = command.ExecuteReader();

    int recordCount = 0;

    while (reader.Read())
    {
        var columns = new List<string>();
        var values = new List<string>();

        for (int i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i);

            columns.Add($"\"{columnName.Replace("\"", "\"\"")}\"");

            if (reader.IsDBNull(i))
            {
                values.Add("NULL");
            }
            else
            {
                object value = reader.GetValue(i);

                switch (value)
                {
                    case byte[] bytes:
                        values.Add($"X'{Convert.ToHexString(bytes)}'");
                        break;

                    case string text:
                        values.Add($"'{text.Replace("'", "''")}'");
                        break;

                    case long:
                    case int:
                    case short:
                    case double:
                    case float:
                        values.Add(Convert.ToString(
                            value,
                            System.Globalization.CultureInfo.InvariantCulture
                        )!);
                        break;

                    default:
                        values.Add(
                            $"'{value.ToString()!.Replace("'", "''")}'"
                        );
                        break;
                }
            }
        }

        sql.AppendLine(
            $"INSERT INTO \"{table}\" ({string.Join(", ", columns)}) " +
            $"VALUES ({string.Join(", ", values)});"
        );

        recordCount++;
    }

    sql.AppendLine();
    Console.WriteLine($"Exported {recordCount} records.");
}

sql.AppendLine("COMMIT;");

Directory.CreateDirectory(
    Path.GetDirectoryName(outputPath)!
);

File.WriteAllText(
    outputPath,
    sql.ToString(),
    new UTF8Encoding(false)
);

Console.WriteLine();
Console.WriteLine("DML export completed successfully!");
Console.WriteLine($"Output: {outputPath}");

Console.WriteLine();
Console.WriteLine("Starting database reconstruction test...");

CareerTrack.DbExport.DatabaseVerifier.Verify(solutionDirectory);
