using System.Collections.Generic;
using System.Linq;

namespace ESchool.Domain.Models
{
    public class ServerResult
    {
        public ServerResult()
            : this(ErrorCode.Success)
        {
        }

        public ServerResult(ErrorCode code)
            : this(code, Enumerable.Empty<string>())
        {
        }

        public ServerResult(ErrorCode code, IEnumerable<string> message)
        {
            Code = (int)code;
            Messages = message;
        }

        public int Code { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }
}
