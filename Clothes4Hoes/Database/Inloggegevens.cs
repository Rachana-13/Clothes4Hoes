namespace SchoolTemplate.Database
{
  public class Inloggegevens
  {
    public int Klant_id { get; set; }

    public string Voornaam { get; set; }

    public string Achternaam { get; set; }
    public string Emailadres { get; set; }

    public string Wachtwoord { get; set; }

    public System.DateTime? Geboortedatum { get; set; }

  }
}
