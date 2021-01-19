using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ErrorResponse : Exception
    {
        public ErrorResponse()
        {

        }

        public ErrorResponse(ErrorObj internalServerError, ErrorObj notFound, ErrorObj badRequest, string stack_trace, string message, List<object> data, string inner_exception, string help_link, string source, string h_result)
        {
            InternalServerError = internalServerError;
            NotFound = notFound;
            BadRequest = badRequest;
            Stack_trace = stack_trace;
            Message = message;
            Data = data;
            Inner_exception = inner_exception;
            Help_link = help_link;
            Source = source;
            H_result = h_result;
        }

        public ErrorObj InternalServerError { get; set; } = new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.",description: "Internal Server Error");
        public ErrorObj NotFound { get; set; } = new ErrorObj(code: 404, message: "The server can not find the requested resource.", description: "Not Found");
        public ErrorObj BadRequest { get; set; } = new ErrorObj(code: 400, message: "The server could not understand the request due to invalid syntax.",description: "Bad Request");

        public ErrorObj Unauthorized { get; set; } = new ErrorObj(code: 401, message: "Unauthorized.", description: "Unauthorized");

        public ErrorObj Error;
        public String Stack_trace { get; set; }
        public String Message { get; set; }
        public List<Object> Data { get; set; }
        public String Inner_exception { get; set; }
        public String Help_link { get; set; }
        public String Source { get; set; }
        public String H_result { get; set; }


    }

}
