using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CreatureScriptsParser
{
    public static class Sql
    {
        public static DataSet WorldDatabaseSelectQuery(string query)
        {
            DataSet dataSet = new DataSet();
            MySqlConnection sqlConnection = new MySqlConnection();
            sqlConnection.ConnectionString = "server = localhost;" + " port = 3306;" + " user id = root;" + " password = root;" + " database = world;";
            try
            {
                sqlConnection.Open();
                MySqlCommand myCommand = new MySqlCommand(query, sqlConnection);
                MySqlDataAdapter DataAdapter = new MySqlDataAdapter();
                DataAdapter.SelectCommand = myCommand;
                DataAdapter.Fill(dataSet, "table");
                return dataSet;
            }
            catch (MySqlException myerror)
            {
                MessageBox.Show("Error Connecting to Database: " + myerror.Message, "Database Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return null;
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public static DataSet HotfixDatabaseSelectQuery(string query)
        {
            DataSet dataSet = new DataSet();
            MySqlConnection sqlConnection = new MySqlConnection();
            sqlConnection.ConnectionString = "server = localhost;" + " port = 3306;" + " user id = root;" + " password = root;" + " database = hotfix;";
            try
            {
                sqlConnection.Open();
                MySqlCommand myCommand = new MySqlCommand(query, sqlConnection);
                MySqlDataAdapter DataAdapter = new MySqlDataAdapter();
                DataAdapter.SelectCommand = myCommand;
                DataAdapter.Fill(dataSet, "table");
                return dataSet;
            }
            catch (MySqlException myerror)
            {
                MessageBox.Show("Error Connecting to Database: " + myerror.Message, "Database Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return null;
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public static uint? GetSpellDurationForSpell(uint spell)
        {
            DataSet dataSet = new DataSet();
            MySqlConnection sqlConnection = new MySqlConnection();
            sqlConnection.ConnectionString = "server = localhost;" + " port = 3306;" + " user id = root;" + " password = root;" + " database = db2;";
            try
            {
                sqlConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("SELECT `DurationIndex` FROM `spellmisc` WHERE `SpellId` = " + spell + ";", sqlConnection);
                MySqlDataAdapter DataAdapter = new MySqlDataAdapter();
                DataAdapter.SelectCommand = myCommand;
                DataAdapter.Fill(dataSet, "table");

                if (dataSet.Tables["table"].Rows.Count != 0)
                {
                    return Convert.ToUInt32(dataSet.Tables["table"].Select().First().ItemArray.First());
                }
            }
            catch (MySqlException myerror)
            {
                MessageBox.Show("Error Connecting to Database: " + myerror.Message, "Database Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            return null;
        }
    }
}
