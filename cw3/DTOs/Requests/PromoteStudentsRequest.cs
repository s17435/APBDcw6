using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;


namespace cw4.DTOs.Requests
{
    
    
    
    public class PromoteStudentsRequest
    {
        [Required(ErrorMessage = "Musisz podać nazwę studiów")]
        public string Studies { get; set; }
        
        [Required(ErrorMessage = "Musisz podać numer semestru")]
        public int Semester { get; set; }
    }
}