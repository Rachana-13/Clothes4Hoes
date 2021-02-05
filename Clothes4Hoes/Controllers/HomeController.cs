using Clothes4Hoes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SchoolTemplate.Database;
using SchoolTemplate.Models;

namespace SchoolTemplate.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = "Server=172.16.160.21;Port=3306;Database=109807;Uid=109807;Pwd=rfultyRa;";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            return View();

        [Route("kledingstuk/{Id}")]
            
        public IActionResult Kledingstuk(string id)
        { 
                var model = GetKledingstuk(id);
                var Festivaldagen = GetKledingstukInfo(id);
                ViewData["kledingstukinfo"] = KledingstukInfo;

                return View(model);
        }
        
        [Route("Inloggen")]
        public IActionResult Inloggen()
        {
            return View();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        private void SavePerson(PersonModel person)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO klant(naam,achternaam,emailadres,geboortedatum) VALUEs(?voornaam,?achternaam,?emailadres,?geboortedatum)", conn);
                cmd.Parameters.Add("?voornaam", MySqlDbType.VarChar).Value = person.Voornaam;
                cmd.Parameters.Add("?achternaam", MySqlDbType.VarChar).Value = person.Achternaam;
                cmd.Parameters.Add("?emailadres", MySqlDbType.VarChar).Value = person.Email;
                cmd.Parameters.Add("?geboortedatum", MySqlDbType.Date).Value = person.Geboortedatum;
                cmd.ExecuteNonQuery();
            }
        }

        [Route("Kleding")]
        public IActionResult Kleding()
        {
            return View();
        }


            private List<Kledingstuk> GetKledingstuk()
            {

                List<Kledingstuk> kledingstuks = new List<Kledingstuk>();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("select * from kledingstuk", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Kledingstuk f = new Kledingstuk
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Naam = reader["Naam"].ToString(),
                                Beschrijving = reader["Beschrijving"].ToString(),
                                Prijs = reader["Prijs"].ToString(),
                                Afbeelding = reader["Afbeelding"].ToString(),

                            };

                            kledingstuks.Add(f);
                        }
                    }
                }

                return kledingstuks;
            }

            //zet data neer van de database festival
            private Kledingstuk GetKledingstuk(string id)
            {
                List<Kledingstuk> kledingstuks = new List<Kledingstuk>();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand($"select * from kledingstuk where id = {id}", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Kledingstuk f = new Kledingstuk
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Naam = reader["Naam"].ToString(),
                                Beschrijving = reader["Beschrijving"].ToString(),
                                Afbeelding = reader["Afbeelding"].ToString(),
                                Prijs = reader["Prijs"].ToString(),

                            };
                            kledingstuks.Add(f);
                        }
                    }
                }

                return kledingstuks[0];
            }
            private List<KledingstukInfo> GetKledingstukInfo(string kledingstukId)
            {
                List<KledingstukInfo> kledingstuks = new List<KledingstukInfo>();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand($"select * from kledingstuk_info where kledingstuk_id = {kledingstukId}", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            KledingstukInfo f = new KledingstukInfo
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Festival_id = Convert.ToInt32(reader["Kledingstuk_id"]),
                                Datum = DateTime.Parse(reader["Maten"].ToString()),
                                Start = reader["Voorraad"].ToString(),
                            };
                            kledingstuks.Add(f);
                        }
                    }
                }

                return kledingstuks;
            }
        }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
