
using Microsoft.Data.Sqlite;

namespace CareerTrack.DbExport
{
    public static class DatabaseVerifier
    {
        public static void Verify(string solutionDirectory)
        {
            string scriptsDirectory = Path.Combine(
                solutionDirectory, "CareerTrack", "Scripts");

            string ddlPath = Path.Combine(scriptsDirectory, "DDL.sql");
            string dmlPath = Path.Combine(scriptsDirectory, "DML.sql");

            string testDirectory = Path.Combine(
                solutionDirectory, "CareerTrack.DbExport", "TestOutput");

            string testDatabasePath = Path.Combine(
                testDirectory, "careertrack_test.db");

            if (!File.Exists(ddlPath) || !File.Exists(dmlPath))
            {
                Console.WriteLine("DDL.sql or DML.sql was not found.");
                return;
            }

            Directory.CreateDirectory(testDirectory);

            // Never overwrite an existing database file.
            if (File.Exists(testDatabasePath))
            {
                Console.WriteLine(
                    "Test database already exists. Verification stopped.");
                return;
            }

            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = testDatabasePath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                ForeignKeys = true
            }.ToString();

            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                Console.WriteLine("Creating database tables...");

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = File.ReadAllText(ddlPath);
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Inserting sample records...");

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = File.ReadAllText(dmlPath);
                    command.ExecuteNonQuery();
                }

                string[] tables =
                {
                    "Companies",
                    "ApplicationStatuses",
                    "Contacts",
                    "JobApplications",
                    "Interviews"
                };

                int totalRecords = 0;

                Console.WriteLine();
                Console.WriteLine("Database verification results:");

                foreach (string table in tables)
                {
                    using var command = connection.CreateCommand();

                    command.CommandText =
                        $"SELECT COUNT(*) FROM \"{table}\";";

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    Console.WriteLine($"{table}: {count} records");

                    totalRecords += count;
                }

                Console.WriteLine();
                Console.WriteLine($"Total records: {totalRecords}");

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "PRAGMA foreign_key_check;";

                    using var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine(
                            "FAILED: Foreign-key violations detected.");

                        while (reader.Read())
                        {
                            Console.WriteLine(
                                $"Table: {reader.GetString(0)}, Row: {reader.GetValue(1)}");
                        }

                        return;
                    }
                }

                if (totalRecords >= 30)
                {
                    Console.WriteLine(
                        "PASSED: Database reconstructed successfully.");
                    Console.WriteLine(
                        "PASSED: At least 30 records exist.");
                    Console.WriteLine(
                        "PASSED: No foreign-key violations.");
                }
                else
                {
                    Console.WriteLine(
                        "FAILED: Fewer than 30 records were found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Database verification failed: {ex.Message}");
            }
        }
    }
}
