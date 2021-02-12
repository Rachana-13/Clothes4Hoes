using Clothes4Hoes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Threading.Tasks;

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
        [Route("Kleding")]

        public IActionResult Kleding()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("kledingstuk/{Id}")]
        public IActionResult Kledingstuk(string id)
        {
            var model = GetKledingstuk(id);
            var Kledingstuks = GetKledingstuks(id);
            ViewData["kledingstuks"] = Kledingstuks;

            return View(model);
        }
        private List<Kledingstuk> GetKledingstuks(string id)
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
                        Kledingstuk k = new Kledingstuk
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Prijs = reader["Prijs"].ToString(),
                            Afbeelding = reader["Afbeelding"].ToString(),

                        };

                        kledingstuks.Add(k);
                    }
                }
            }

            return kledingstuks;
        }
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
                        Kledingstuk k = new Kledingstuk
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Afbeelding = reader["Afbeelding"].ToString(),
                            Prijs = reader["Prijs"].ToString(),

                        };
                        kledingstuks.Add(k);
                    }
                }
            }

            return kledingstuks[0];
        }
    }
}
