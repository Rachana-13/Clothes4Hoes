using Clothes4Hoes.Models;
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
using Microsoft.AspNetCore.Http;

namespace SchoolTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string connectionString = "Server=informatica.st-maartenscollege.nl;Port=3306;Database=109807;Uid=109807;Pwd=rfultyRa;";
        // private readonly string connectionString = "Server=172.16.160.21;Port=3306;Database=109807;Uid=109807;Pwd=rfultyRa;";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
          ViewData["UserName"] = HttpContext.Session.GetString("UserName");

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("kledingstuk/{Id}")]
        public IActionResult Kledingstuk(string id)
        {
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

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

        [Route("registreren")]
        public IActionResult Registreren()
        {

          return View();
        }


        [Route("registreren")]
        [HttpPost]
        public IActionResult Registreren(KlantModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            SavePersonLogIn(model);

            return Redirect("/inlogpagina");
        }


        [Route("login")]
        public IActionResult Login()
        {         
          return View();
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(string email, string wachtwoord)
        {
          var klant = GetPersonByEmail(email);

          if (klant.Wachtwoord == wachtwoord)
          {
              HttpContext.Session.SetInt32("UserId", klant.Id);
              HttpContext.Session.SetString("UserName", klant.Voornaam);
              return Redirect("/profiel");
          }
      
          return View();
        }

        [Route("profiel")]
        public IActionResult Profiel()
        {
          int? id = HttpContext.Session.GetInt32("UserId");

          var model = GetPersonById(id.Value);

          return View(model);
        }

    //data naar database sturen vanuit contactformulier
        private void SavePerson(PersonModel person){
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO klant_contact(voornaam,achternaam,emailadres,geboortedatum) VALUEs(?voornaam,?achternaam,?emailadres,?geboortedatum)", conn);
                cmd.Parameters.Add("?voornaam", MySqlDbType.VarChar).Value = person.Voornaam;
                cmd.Parameters.Add("?achternaam", MySqlDbType.VarChar).Value = person.Achternaam;
                cmd.Parameters.Add("?emailadres", MySqlDbType.VarChar).Value = person.Email;
                cmd.Parameters.Add("?geboortedatum", MySqlDbType.Date).Value = person.Geboortedatum;
                cmd.ExecuteNonQuery();
            }
        }


    private KlantModel GetPersonByEmail(string email)
    {
      List<KlantModel> persons = new List<KlantModel>();

      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        conn.Open();
        MySqlCommand cmd = new MySqlCommand($"select * from klant_inloggen where emailadres = '{email}'", conn);

        using (var reader = cmd.ExecuteReader())
        {
          while (reader.Read())
          {
            KlantModel p = new KlantModel
            {
              Id = Convert.ToInt32(reader["Id"]),
              Voornaam = reader["voornaam"].ToString(),
              Achternaam = reader["achternaam"].ToString(),
              Email = reader["emailadres"].ToString(),
              Wachtwoord = reader["wachtwoord"].ToString()

            };
            persons.Add(p);
          }
        }
      }

      return persons[0];
    }


        private KlantModel GetPersonById(int id)
        {
          List<KlantModel> persons = new List<KlantModel>();

          using (MySqlConnection conn = new MySqlConnection(connectionString))
          {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand($"select * from klant_inloggen where id = {id}", conn);

            using (var reader = cmd.ExecuteReader())
            {
              while (reader.Read())
              {
                KlantModel p = new KlantModel
                {
                  Id = Convert.ToInt32(reader["Id"]),
                  Voornaam = reader["voornaam"].ToString(),
                  Achternaam = reader["achternaam"].ToString(),
                  Email = reader["emailadres"].ToString()
                };
                persons.Add(p);
              }
            }
          }

          return persons[0];
        }

        //data naar database sturen vanuit regsitratieformulier
        private void SavePersonLogIn(KlantModel personlogin)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO klant_inloggen(voornaam,achternaam,emailadres,geboortedatum,wachtwoord) VALUEs(?voornaam,?achternaam,?emailadres,?geboortedatum,?wachtwoord)", conn);
                    cmd.Parameters.Add("?voornaam", MySqlDbType.VarChar).Value = personlogin.Voornaam;
                    cmd.Parameters.Add("?achternaam", MySqlDbType.VarChar).Value = personlogin.Achternaam;
                    cmd.Parameters.Add("?emailadres", MySqlDbType.VarChar).Value = personlogin.Email;
                    cmd.Parameters.Add("?geboortedatum", MySqlDbType.Date).Value = personlogin.Geboortedatum;
                    cmd.Parameters.Add("?wachtwoord", MySqlDbType.VarChar).Value = personlogin.Wachtwoord;
             
                    cmd.ExecuteNonQuery();
                }
            }

    }

}


   