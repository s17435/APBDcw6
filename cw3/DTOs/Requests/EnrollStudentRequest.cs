using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace cw4.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required(ErrorMessage = "Musisz podać nr indeksu")]
        [RegularExpression("^s[0-9]+$")]
        public string IndexNumber { get; set; }

        // Najdłuższe imie świata zawiera 81 liter, więc uznałem 100 za limit
        [Required(ErrorMessage = "Musisz podać imię")]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        
        
        // Najdłuższe nazwisko w Polsce to 27 liter, prawo Polskie pozwala tylko na 2 nazwiska, limit 100 znaków
        [Required(ErrorMessage = "Musisz podać nazwisko")]
        [MaxLength(100)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Musisz podać datę urodzenia")]
        public string BirthDate { get; set; }
        
        [Required(ErrorMessage = "Musisz podać nazwę studiów")]
        public string Studies { get; set; }
       
       
    }
}