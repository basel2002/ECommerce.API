using System.Text.Json.Serialization;

namespace Common
{
    public class GeneralResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string ,List<Errors>>? Errors { get; set; }


        public static GeneralResult SuccessResult(string message = "Success")
            =>new() { Success = true, Message = message ,Errors = null};
        public static GeneralResult FailResult(string message = "Failed")
        => new() { Success = false, Message = message , Errors = null};

        public static GeneralResult FailResult(Dictionary<string,List<Errors>> errors, string message = "Failed")
        => new() { Success = false, Message = message, Errors = errors };

        public static GeneralResult NotFound(string message = "Not Found")
            => new() { Success = false, Message = message,Errors = null }; 

    }



    public class GeneralResult<T>:GeneralResult
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        public static GeneralResult<T> SuccessResult(T data, string message = "Success")
        => new() { Success = true, Message = message, Data = data, Errors = null };
        public static new GeneralResult<T> SuccessResult(string message = "Success")
          => new() { Success = true, Message = message, Errors = null };
        public static new GeneralResult<T> FailResult(string message = "Failed")
        => new() { Success = false, Message = message, Errors = null };

        public static new GeneralResult<T> FailResult(Dictionary<string, List<Errors>> errors, string message = "Failed")
        => new() { Success = false, Message = message, Errors = errors };

        public static new  GeneralResult<T> NotFound(string message = "Not Found")
            => new() { Success = false, Message = message, Errors = null };


    }
}
