using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Clothes4Hoes.Models
// namespace SchoolTemplate.Models
{
    public class InlogModel
    {
        //error messages als de invoer fout is 
        [Required(ErrorMessage = "Voornaam is verplicht!")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht!")]
        public string Achternaam { get; set; }

        [Required(ErrorMessage ="E-mail is verplicht!")]
        [EmailAddress(ErrorMessage = "Geen geldig e-mailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Geboortedatum is verplicht!")]
        public DateTime? Geboortedatum { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht!")]
        public string Wachtwoord { get; set; }

       

    }
}
