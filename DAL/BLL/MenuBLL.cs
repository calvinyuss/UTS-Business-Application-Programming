using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;

namespace DAL.BLL
{
    public class MenuBLL : Connection
    {

        public string savedImageFileName { get; set; }

        public MenuBLL() : base()
        {
            
        }

        public List<Menu> fetchMenu()
        {
            DB_DATAEntities _db = new DB_DATAEntities();


            IQueryable<Menu> menus = from menu in _db.Menus
                        where menu.deleted_at == null
                        select menu;

            return menus.ToList();
        }

        public List<Menu> fetchMenuSortByNameAsc()
        {
            DB_DATAEntities _db = new DB_DATAEntities();


            IQueryable<Menu> menus = from menu in _db.Menus
                                     where menu.deleted_at == null
                                     orderby menu.name ascending
                                     select menu;

            return menus.ToList();
        }

        public List<Menu> fetchMenuSortByNameDesc()
        {
            DB_DATAEntities _db = new DB_DATAEntities();


            IQueryable<Menu> menus = from menu in _db.Menus
                                     where menu.deleted_at == null
                                     orderby menu.name descending
                                     select menu;

            return menus.ToList();
        }

        public List<Menu> fetchMenuSortByPriceDesc()
        {
            DB_DATAEntities _db = new DB_DATAEntities();


            IQueryable<Menu> menus = from menu in _db.Menus
                                     where menu.deleted_at == null
                                     orderby menu.price descending
                                     select menu;

            return menus.ToList();
        }

        public List<Menu> fetchMenuSortByPriceAsc()
        {
            DB_DATAEntities _db = new DB_DATAEntities();


            IQueryable<Menu> menus = from menu in _db.Menus
                                     where menu.deleted_at == null
                                     orderby menu.price ascending
                                     select menu;

            return menus.ToList();
        }

        public Menu getMenu(int id)
        {
            DB_DATAEntities _db = new DB_DATAEntities();
            IQueryable<Menu> menus = from menu in _db.Menus
                                     where (menu.deleted_at == null) && (menu.id == id)
                                     select menu;
            return menus.FirstOrDefault();
        }


        public List<Menu> topSales()
        {
            List<Menu> menus = new List<Menu>();
            string queryString =
            @"SELECT id, name, price, img_url 
                FROM
                (
                    SELECT menu_id, COUNT(menu_id) as sale

                    FROM order_items    

                    GROUP BY menu_id
                ) AS weeklyMenuSales
                INNER JOIN menus
                on menus.id = weeklyMenuSales.menu_id
                WHERE menus.deleted_at IS NULL
                ORDER BY sale DESC";

            using (connection)
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DAL.Menu menu = new DAL.Menu();

                        menu.id = (int)reader[0];
                        menu.name = (string)reader[1];
                        menu.price = (decimal)reader[2];
                        menu.img_url = (string)reader[3];

                        menus.Add(menu);
                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return menus;
            }

        }

        public List<Menu> getWeeklyTopSales(){
            List<Menu> menus = new List<Menu>();
            string queryString =
            @"SELECT id, name, price, img_url 
                FROM
                (
                    SELECT menu_id, COUNT(menu_id) as sale

                    FROM order_items

                    WHERE create_at BETWEEN dateadd(day, -7, getdate()) AND getdate()

                    GROUP BY menu_id
                ) AS weeklyMenuSales
                INNER JOIN menus
                on menus.id = weeklyMenuSales.menu_id
                WHERE menus.deleted_at IS NULL
                ORDER BY sale DESC";

            using (connection)
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DAL.Menu menu = new DAL.Menu();

                        menu.id = (int)reader[0];
                        menu.name = (string)reader[1];
                        menu.price = (decimal)reader[2];
                        menu.img_url = (string)reader[3];

                        menus.Add(menu);
                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                   return menus;
            }
        }

        public void handleCreate(string name, double price, string url_image)
        {
            try
            {
                bool  validator = validateInput(name, price, url_image);

                if (validator)
                {
                    ImageServices imageService = new ImageServices(url_image);
                    imageService.storeImage();

                    savedImageFileName = imageService.createdFileName;

                    createToDatabase(name, price, savedImageFileName);
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void handleUpdate(int id, string name, double price, string url_image)
        {
            try
            {
                bool validator = validateInput(name, price, url_image);

                if (validator)
                {
                    updateToDatabase(id, name, price, url_image);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void updateToDatabase(int id, string name, double price, string url_image)
        {
            connection.Open();
            string query = "UPDATE menus " +
                "SET name=@name," +
                "price=@price," +
                "img_url=@url_image " +
                "WHERE id=@id";
            try
            {
                SqlCommand command = new SqlCommand(query, connection);

                ImageServices imageService = new ImageServices(url_image);
                imageService.storeImage();

                savedImageFileName = imageService.createdFileName;

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@url_image", Path.GetFileName(savedImageFileName));

                command.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private bool validateInput(string name, double price, string url_image)
        {
            Console.WriteLine(url_image);
            if (name.Count() == 0) throw new Exception("Name input is empty");

            if (price == 0) throw new Exception("Price can't empty");

            if ( !File.Exists(url_image) ) throw new Exception("Image not found");

            return true;
        }

        public void createToDatabase(string name, double price, string url_image)
        {

            connection.Open();
            string query = "INSERT INTO menus (name, price, img_url) " +
                "VALUES (@name, @price, @url_image)";
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@url_image", Path.GetFileName(url_image));

                command.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void remove(int id)
        {
            connection.Open();
            string query = "UPDATE menus SET deleted_at=getDate() WHERE id=@id";
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public Menu getLastInsertedData()
        {
            DB_DATAEntities _db = new DB_DATAEntities();

            IQueryable<Menu> menus = from menu in _db.Menus
                                     where menu.deleted_at == null
                                     orderby menu.id descending
                                     select menu;
            return menus.First();
        }
    }
}
