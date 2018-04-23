using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication5.Interfaces;
using WebApplication5.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication5.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<ApplicationUser> UsersRepo { get; }
        IBaseRepository<FriendRequest> RequestsRepo { get; }
        IBaseRepository<FriendShip> FriendsRepo { get; }
        int Save();
    }
}
