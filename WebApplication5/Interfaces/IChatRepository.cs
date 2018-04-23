using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication5.Interfaces
{
    public interface IChatRepository
    {
        void Add(string name, string message);
    }
}
