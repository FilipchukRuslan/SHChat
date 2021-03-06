﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication5.Interfaces;
using WebApplication5.Models;
using WebApplication5.Repositories;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApplication5
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        private IBaseRepository<FriendRequest> requestsRepo;
        private IBaseRepository<ApplicationUser> usersRepo;
        private IBaseRepository<FriendShip> friendsRepo;
        private IBaseRepository<Message> messagesRepo;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IBaseRepository<ApplicationUser> UsersRepo
        {
            get
            {
                if (usersRepo == null) { usersRepo = new BaseRepository<ApplicationUser>(context); }
                return usersRepo;
            }
        }

        public IBaseRepository<FriendRequest> RequestsRepo
        {
            get
            {
                if (requestsRepo == null) { requestsRepo = new BaseRepository<FriendRequest>(context); }
                return requestsRepo;
            }
        }

        public IBaseRepository<FriendShip> FriendsRepo
        {
            get
            {
                if (friendsRepo == null) { friendsRepo = new BaseRepository<FriendShip>(context); }
                return friendsRepo;
            }
        }

        public IBaseRepository<Message> MessagesRepo
        {
            get
            {
                if (messagesRepo == null) { messagesRepo = new BaseRepository<Message>(context); }
                return messagesRepo;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        private bool isDisposed = false;

        protected virtual void Grind(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            isDisposed = true;
        }

        public void Dispose()
        {
            Grind(true);
            GC.SuppressFinalize(this);
        }
    }
}