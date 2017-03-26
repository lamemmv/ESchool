using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ESchool.DomainModels.Models
{
	public class ServerResult
    {
        public ServerResult()
            : this(HttpStatusCode.OK)
        {
        }

        public ServerResult(HttpStatusCode code)
            : this(code, Enumerable.Empty<string>())
        {
        }

        public ServerResult(HttpStatusCode code, IEnumerable<string> message)
        {
            Code = (int)code;
            Messages = message;
        }

        public int Code { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }
}
