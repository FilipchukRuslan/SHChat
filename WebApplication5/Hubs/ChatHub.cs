using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using WebApplication5.Interfaces;
using WebApplication5.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApplication5.Hubs
{
    public class ChatHub : Hub
    {
        private static List<UserDTO> ConnectedClientsList;

        private IUnitOfWork unitOfWork;

        public ChatHub(IUnitOfWork unitofwork)
        {
            unitOfWork = unitofwork;
        }

        [Authorize]
        public override Task OnConnected()
        {
            if (ConnectedClientsList == null)
            {
                ConnectedClientsList = new List<UserDTO>();
            }
            try
            {
                var user = ConnectedClientsList.Find(c => c.UserId == Context.User.Identity.GetUserId());
                user.ConnectionId = Context.ConnectionId;
                user.Status = true;
                ChatUsers();
            }
            catch (Exception ex)
            {
                if (Context.User.Identity.Name.Count() > 1)
                {
                    ConnectedClientsList.Add(new UserDTO
                    {
                        UserName = Context.User.Identity.Name,
                        ConnectionId = Context.ConnectionId,
                        UserId = Context.User.Identity.GetUserId(),
                        Status = true
                    });
                    ChatUsers();
                }
            }
            return base.OnConnected();
        }

        [Authorize]
        public override Task OnDisconnected(bool endConnection)
        {
            try
            {
                var user = ConnectedClientsList.Find(c => c.UserId == Context.User.Identity.GetUserId());
                user.Status = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Clients.All.showUsers(ConnectedClientsList);
            }
            return base.OnDisconnected(endConnection);
        }
        [Authorize]
        public void ChatUsers()
        {
            Clients.All.showUsers(ConnectedClientsList);
        }

        [Authorize]
        public void SendPrivateToServer(string userConnectionId, string message)
        {
            var toUser = ConnectedClientsList.Where(e => e.ConnectionId == userConnectionId).First().UserName;
            var fromUser = Context.User.Identity.Name;
            unitOfWork.MessagesRepo.Insert(new Message
            {
                FromUser = fromUser,
                MessageText = message,
                ToUser = toUser
            });
            try
            {
                var friendShip = unitOfWork.FriendsRepo.Get()
                    .Where(u => u.User == fromUser && u.Friend == toUser).First();
                Clients.Client(userConnectionId).sendPrivate(message, toUser);
            }
            catch(Exception ex)
            {

            }
            
        }
        [Authorize]
        public void ShowPrivateChat(string userConnectionId)
        {
            Clients.Caller.openChat(userConnectionId);
        }
        [Authorize]
        public void HistoryShow(string from)
        {
            var messages = unitOfWork.MessagesRepo.Get().Where(e => e.FromUser == from && e.ToUser == Context.User.Identity.Name);
            Clients.Caller.sendHistory(messages);
        }
        [Authorize]
        public void SendRequest(string toIdCon)
        {
            var user = ConnectedClientsList.Where(e => e.ConnectionId == toIdCon).First().UserName;
            FriendRequest req = new FriendRequest
            {
                FromUser = Context.User.Identity.GetUserId(),
                ToUser = user
            };

            unitOfWork.UsersRepo
                .GetAll()
                .ToList()
                .Find(c => c.Email == ConnectedClientsList.Find(a => a.ConnectionId == toIdCon).UserName)
                .FriendRequests.Add(req);
            unitOfWork.Save();
            var reqId = unitOfWork.RequestsRepo.GetAll().ToList().Find(c => c.FromUser == req.FromUser && c.ToUser == req.ToUser).Id;
            Clients.Client(toIdCon).sendRequestTo(req.FromUser, reqId);

        }

        [Authorize]
        public void Answer(string friendName, int reqId, bool answer)
        {
            if (answer)
            {
                FriendShip friendShip = new FriendShip
                {
                    User = Context.User.Identity.Name,
                    Friend = friendName
                };
                unitOfWork.FriendsRepo.Insert(friendShip);
                unitOfWork.Save();
            }
            else
            {
                var delreq = unitOfWork.RequestsRepo.GetById(reqId);
                unitOfWork.RequestsRepo.Delete(delreq);
                unitOfWork.Save();
            }
        }
    }
}