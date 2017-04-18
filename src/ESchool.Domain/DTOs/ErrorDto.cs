using ESchool.Domain.Enums;

namespace ESchool.Domain.DTOs
{
    public sealed class ErrorDto
    {
        public ErrorDto(ErrorCode code)
            : this(code, null, null)
        {
        }

        public ErrorDto(ErrorCode code, string message)
            : this(code, message, null)
        {
        }

        public ErrorDto(ErrorCode code, string message, object data)
        {
            Code = (int)code;
            Message = message;
            Data = data;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
