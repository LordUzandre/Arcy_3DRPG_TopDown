using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public class SpliteExampleSimple : MonoBehaviour
{
    public void OnEnable()
    {
        string connectionString = "URI=file:" + Application.dataPath + "/Data/Dialogue/DB_Debug-scene.db";
        string dialogue01 = "Dialogue01";
        string speakerID = "001";

        // Connect to the SQLite database
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        
        // Open the database connection and create a command to execute SQL queries
        dbConnection.Open();        
        IDbCommand dbCommand = dbConnection.CreateCommand();

        // Select all data from row specified by {keyvalue}
        dbCommand.CommandText = $"SELECT DISTINCT char, english FROM {dialogue01} WHERE speakID = {speakerID}";

        // Execute the query and retrieve the result
        IDataReader reader = dbCommand.ExecuteReader();

        // Check if there are rows in the result
        while (reader.Read())
        {
            // Retrieve the content of the 5th column (assuming it's a string)
            string content = reader.GetString(1); // Note: SQLite indices are zero-based

            // Log the content to the console
            Debug.Log(content);
        }

        // Close the connections
        reader.Close();
        dbCommand.Dispose();
        dbConnection.Close();
    }
}