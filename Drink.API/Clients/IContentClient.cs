using Drink.API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drink.API.Clients
{
    public interface IContentClient
    {
        Task<T> SendGetAsync<T>(OperationType operation, params object[] fields);
    }
}
