﻿using Clothes4Hoes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Linq;
using System.Threading.Tasks;
using SchoolTemplate.Database;

namespace SchoolTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=109807;Uid=109807;Pwd=rfultyRa;";
        // private readonly string connectionString = "Server=172.16.160.21;Port=3306;Database=109807;Uid=109807;Pwd=rfultyRa;";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var kledingstuks = GetKledingstuks();
            return View(kledingstuks);
        }

        public List<Kledingstuk> GetKledingstuks()
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
    
       
        public IActionResult Privacy ()
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
            var Kledingstukinfos = GetKledingstukInfo(id);
            ViewData["kledingstukinfos"] = Kledingstukinfos;

            return View(model);
        }

        [Route("Kleding")]

         public IActionResult Kleding()
        {
            return View(GetKledingstuks());
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
                        KledingstukInfo k = new KledingstukInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Kledingstuk_id = Convert.ToInt32(reader["Kledingstuk_id"]),
                            Maten = reader["Maten"].ToString(),
                            Voorraad = reader["Voorraad"].ToString(),
                        };
                        kledingstuks.Add(k);
                    }
                }
            }

            return kledingstuks;
        }

        private readonly ILogger<HomeController> _logger;


        [Route("Inloggen")]

        public IActionResult Inloggen()
        {
            return View();
        }
      
        //redirect naar contact
        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();

        }
        //als het contactformulier goed is ingevuld volgens de requirements dan wordt je naar gelukt gestuurd
        [Route("Contact")]
        [HttpPost]
        public IActionResult Contact(PersonModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            SavePerson(model);

            return Redirect("/gelukt");
        }
        [Route("gelukt")]
        public IActionResult Gelukt()
        {
            return View();
        }
        //data naar database sturen
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



    }

}


   