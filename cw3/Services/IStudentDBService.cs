using cw4.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw4.DTOs.Responses;

namespace cw4.Services
{
    public interface IStudentDBService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        PromoteStudentsResponse PromoteStudent(PromoteStudentsRequest request);

    }
}