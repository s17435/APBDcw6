using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using cw4.Models;


namespace cw4.DTOs.Responses
{
    public class PromoteStudentsResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public Enrollment enrollment { get; set; }
    }
}