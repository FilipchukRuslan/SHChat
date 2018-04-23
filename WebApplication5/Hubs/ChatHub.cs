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
        private static List<Message> Messages = new List<Message>();

        private IUnitOfWork unitOfWork;

        public ChatHub(IUnitOfWork unitofwork)
        {
            unitOfWork = unitofwork;
        }
        public override Task OnConnected()
        {
            if (ConnectedClientsList == null)
            {
                ConnectedClientsList = new List<UserDTO>();
            }
            var user = ConnectedClientsList.Find(c => c.UserId == Context.User.Identity.GetUserId());
            if (Context.User.Identity.Name.Count() != 0 && user==null)
            {
                ConnectedClientsList.Add(new UserDTO
                {
                    UserName = Context.User.Identity.Name,
                    ConnectionId = Context.ConnectionId,
                    UserId = Context.User.Identity.GetUserId(),
                    Status = true
                });
            }

            ChatUsers();
            return base.OnConnected();
        }

        public void Send(string message)
        {
            Clients.All.message(Context.User.Identity.GetUserName() + " " + message);

        }


        public override Task OnDisconnected(bool endConnection)
        {
            var user = ConnectedClientsList.Find(c => c.ConnectionId == Context.ConnectionId);

            if (user.UserName.Count() <= 1)
                ConnectedClientsList.Remove(user);
            else
                user.Status = false;
            Clients.All.showUsers(ConnectedClientsList);
            return base.OnDisconnected(endConnection);
        }

        public void ChatUsers()
        {
            Clients.All.showUsers(ConnectedClientsList);
        }


        public void SendPrivateToServer(string userConnectionId, string message)
        {
            Messages.Add(new Message
            {
                FromUser = Context.User.Identity.Name,
                MessageText = message
            });
            Clients.Client(userConnectionId).sendPrivate(message);
        }
        public void ShowPrivateChat(string userConnectionId)
        {
            Clients.Caller.openChat(userConnectionId);
        }
        public void HistoryShow(string from)
        {
            var messages = Messages.Where(e => e.FromUser == from && e.ToUser == Context.User.Identity.Name);
            Clients.Caller.sendHistory(messages);
        }

        public void SendRequest(string toId)
        {
            var user = unitOfWork.UsersRepo.GetAll().ToList()
                .Find(c => c.Email == ConnectedClientsList.Find(a => a.ConnectionId == toId).UserName).Id;
            FriendRequest req = new FriendRequest
            {
                FromUser = Context.User.Identity.GetUserId(),
                ToUser = user
            };

            unitOfWork.UsersRepo
                .GetAll()
                .ToList()
                .Find(c => c.Email == ConnectedClientsList.Find(a => a.ConnectionId == toId).UserName)
                .FriendRequests.Add(req);
            unitOfWork.Save();
            var reqId = unitOfWork.RequestsRepo.GetAll().ToList().Find(c => c.FromUser == req.FromUser && c.ToUser == req.ToUser).Id;
            Clients.Client(toId).sendRequestTo(req.FromUser, reqId);

        }


        public void Answer(string friendName, int reqId, bool answer)
        {
            if (answer)
            {
                FriendShip user = new FriendShip
                {
                    User = Context.User.Identity.Name,
                    Friend = friendName
                };
                unitOfWork.FriendsRepo.Insert(user);
                var delreq = unitOfWork.RequestsRepo.GetById(reqId);
                unitOfWork.RequestsRepo.Delete(delreq);
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