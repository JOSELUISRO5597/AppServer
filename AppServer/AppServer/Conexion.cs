using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AppServer
{
    public class Conexion
    {
        private String server = "server=ro5597.database.windows.net";
        private String dataBase = "database=AppDB";
        private String uid = "uid=ro5597";
        private String pass = "pwd=Pipesfat1$$";

        public SqlConnection connection { get; }

        public Conexion()
        {
            connection = new SqlConnection();
        }

        /// <summary>
        /// Returns the data necessary for the connection to the database
        /// </summary>
        public String getDataConnection()
        {
            String datosConexion = server + ";" + dataBase + ";" + uid + ";" + pass;
            return datosConexion;
        }

        /// <summary>
        /// Open connection with data base
        /// </summary>
        public Boolean openConnection()
        {
            Boolean flag = true;

            try
            {
                connection.ConnectionString = getDataConnection();
                connection.Open();
            }
            catch (SqlException e)
            {
                flag = false;
            }

            return flag;
        }

        /// <summary>
        /// Return state of Object SqlConnection
        /// </summary>
        /// <returns>String with value "Open" if connection is open, "Closed" if connection is closed </returns>
        public ConnectionState getState()
        {
            return this.connection.State;
        }

    }
}
