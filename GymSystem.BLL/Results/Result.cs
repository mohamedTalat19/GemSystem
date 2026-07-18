using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Results
{
    public sealed record Result(bool success, string? error = null , ResultKind resultKind = ResultKind.Ok)
    {
        public static Result Ok() => new(true);
        
        public static Result Fail(string error) => new(false, error, ResultKind.Conflict);

        public static Result NotFound(string error = "NotFound") => new(false , error , ResultKind.NotFound);

        public static Result Validation(string error = "Vaidayion Error") => new(false, error, ResultKind.Validation);


    }

    public sealed record Result<T>(bool success ,T? value ,string? error = null, ResultKind resultKind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new(true , value);

        public static Result<T> Fail(string error) => new(false,default ,error, ResultKind.Conflict);

        public static Result<T> NotFound(string error = "NotFound") => new(false, default,error, ResultKind.NotFound);

        public static Result<T> Validation(string error = "Vaidayion Error") => new(false,default ,error, ResultKind.Validation);


    }
}
