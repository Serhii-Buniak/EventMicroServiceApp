using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMicroService.Application.Common.Exceptions;

public class ClientException : Exception
{
    public ClientException()
        : base()
    {
    }

    public ClientException(string client)
        : base($"Cannot connect to client {client}")
    {
    }

    public ClientException(string client, Exception innerException)
        : base($"Cannot connect to client {client}", innerException)
    {
    }
}