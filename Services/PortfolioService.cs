using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Protocol;
using web.DTO;
using web.Interface;
using web.Models;
using web.Repo;

namespace web.Services
{
    public class PortfolioService : IPortfolioService
    {

       private readonly  IPortfolioRepo _repo;
       private readonly UserManager<IdentityUser> _usermanager;
        public PortfolioService(IPortfolioRepo repo,UserManager<IdentityUser> usermanager )
        {
            _repo = repo ;
            _usermanager = usermanager;
        }

        public  Task<Stock> AddStock(AddtoPortfolioDTO dto, string userID)
        {
          var stock =  _repo.AddStock(dto, userID); 

         var userPresent = _usermanager.FindByIdAsync(userID);

         if (userPresent == null)
            {
                return null;
            }
             return stock ;
        }
    }
}