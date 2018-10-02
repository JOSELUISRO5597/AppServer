using AppServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AppServer.Helpers
{
    public class DALUserHelper
    {
        private User readCompleteUser(User user, SqlDataReader dataReader)
        {
            user = new User();
            user.Id = (int)dataReader["user_id"];
            user.Name = (string)dataReader["user_name"];
            user.Surname = (string)dataReader["user_surname"];
            user.UserName = (string)dataReader["user_username"];
            user.Email = (string)dataReader["user_email"];
            user.Image = (dataReader["user_photo"] is DBNull) ? string.Empty : (string)dataReader["user_photo"];

            return user;
        }

        public SqlParameter CreateParameter(String ParameterName, object value)
        {
            SqlParameter parameter= new SqlParameter();

            
            parameter.ParameterName = "@" + ParameterName;
            parameter.SqlDbType = SqlDbType.NVarChar;
            parameter.Value = value;

            return parameter;
        }

        public List<User> getUsers()
        {
            List<User> users = new List<User>();
            User user = null;
            Conexion conexion = new Conexion();
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader dataReader;

            try
            {
                sqlCommand.CommandText = "Select user_id,user_name,user_surname,user_username,user_email,user_photo from appuser";
                conexion.openConnection();
                sqlCommand.Connection = conexion.connection;

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    user= readCompleteUser(user,dataReader);
                    users.Add(user);
                }
                dataReader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
                throw;
            }
            finally
            {
                if (conexion.getState() == ConnectionState.Open)
                {
                    conexion.connection.Close();
                }
            }

            return users;
        }


        public User getUser(int id)
        {
            User user = null;
            Conexion conexion = new Conexion();
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader dataReader;

            try
            {
                conexion.openConnection();

                sqlCommand.CommandText = "Select user_id,user_name,user_surname,user_username,user_email,user_photo from appuser where user_id=@Id";
                sqlCommand.Connection = conexion.connection;

                sqlCommand.Parameters.Add(CreateParameter("Id",id));
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    user = readCompleteUser(user, dataReader);
                }

                dataReader.Close();
            }
            catch(SqlException e)
            {
                Console.WriteLine("ERROR: {0}",e.Message);
                throw;
            }
            finally
            {
                if (conexion.getState() == ConnectionState.Open)
                {
                    conexion.connection.Close();
                }
            }


            return user;
            
        }


        public User LoginUser(String Email, String Password)
        {
            User user = null;
            Conexion conexion = new Conexion();
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader dataReader;

            try
            {
                sqlCommand.CommandText = "Select user_id,user_name,user_surname,user_username,user_email,user_photo from appuser where user_email=@Email AND user_password=@Password";
                conexion.openConnection();
                sqlCommand.Connection = conexion.connection;

                sqlCommand.Parameters.Add(CreateParameter("Email", Email));
                sqlCommand.Parameters.Add(CreateParameter("Password", Password));

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    user = readCompleteUser(user, dataReader);
                }

                dataReader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
                throw;
            }
            finally
            {
                if (conexion.getState() == ConnectionState.Open)
                {
                    conexion.connection.Close();
                }
            }


            return user;

        }

        public User RegisterUser(User user)
        {
            Conexion conexion = new Conexion();
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader dataReader;
            SqlParameter parameter = new SqlParameter();

            try
            {
                sqlCommand.CommandText = "insert dbo.appuser(user_name,user_surname,user_username,user_email,user_password,user_photo) values(@Name,@Surname,@UserName,@Email,@Password,@Photo);";
                conexion.openConnection();
                sqlCommand.Connection = conexion.connection;

                sqlCommand.Parameters.Add(CreateParameter("Email", user.Email));
                sqlCommand.Parameters.Add(CreateParameter("Password", user.Password));
                sqlCommand.Parameters.Add(CreateParameter("Name", user.Name));
                sqlCommand.Parameters.Add(CreateParameter("Surname", user.Surname));
                sqlCommand.Parameters.Add(CreateParameter("UserName", user.UserName));
                sqlCommand.Parameters.Add(CreateParameter("Photo", user.Image));

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    user = readCompleteUser(user, dataReader);
                }

                dataReader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
                throw;
            }
            finally
            {
                if (conexion.getState() == ConnectionState.Open)
                {
                    conexion.connection.Close();
                }
            }


            return user;

        }

        public User CheckUserEmail(String Email)
        {
            User user=null;
            Conexion conexion = new Conexion();
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader dataReader;
            SqlParameter parameter = new SqlParameter();

            try
            {
                sqlCommand.CommandText = "Select user_id,user_name,user_surname,user_username,user_email,user_photo from appuser where user_email = @Email";
                conexion.openConnection();
                sqlCommand.Connection = conexion.connection;

                sqlCommand.Parameters.Add(CreateParameter("Email", Email));

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    user = readCompleteUser(user, dataReader);
                }

                dataReader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
                throw;
            }
            finally
            {
                if (conexion.getState() == ConnectionState.Open)
                {
                    conexion.connection.Close();
                }
            }


            return user;

        }

        public Boolean ResetPassword(String Email)
        {
            String password = CreateNewPassword();
            Conexion conexion = new Conexion();
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader dataReader;
            SqlParameter parameter = new SqlParameter();

            try
            {
                sqlCommand.CommandText = "Update appuser set user_password= @Password where user_email=@Email";
                conexion.openConnection();
                sqlCommand.Connection = conexion.connection;

                sqlCommand.Parameters.Add(CreateParameter("Email", Email));
                sqlCommand.Parameters.Add(CreateParameter("Password", password));

                dataReader = sqlCommand.ExecuteReader();
                EnviarCorreo(Email,password);
                dataReader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
                throw;
            }
            finally
            {
                if (conexion.getState() == ConnectionState.Open)
                {
                    conexion.connection.Close();
                }
            }

            return true;
        }

        public String CreateNewPassword()
        {
            int longitud = 8;
            Guid miGuid = Guid.NewGuid();
            string token = Convert.ToBase64String(miGuid.ToByteArray());
            token = token.Replace("=", "").Replace("+", "");
            return token.Substring(0, longitud);
        }

        public void EnviarCorreo(string Email, string Password)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("joseluisruizoliver2@gmail.com", "pipesfat");

            MailMessage mm = new MailMessage("joseluisruizoliver2@gmail.com", Email , "test", "Your new password is: " + Password);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                //Enviamos el mensaje      
                client.Send(mm);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //Aquí gestionamos los errores al intentar enviar el correo
                Console.WriteLine("ERROR MSG");
                throw;
            }
        }
    }
}
