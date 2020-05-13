using Giori_Consul.Models;
using Giori_Consul.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Giori_Consul.Services
{
    public class UserServies
    {
        SqlDbContext db = new SqlDbContext();
        public RetornoServiceUser InserUsuario(RoleEnum tipoUsuario, string login, string senha, string nomeUser)
        {
            RetornoServiceUser response = new RetornoServiceUser();
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
            {
                response.MsgError = "Favor preencher login e senha";
                response.Sucess = false;
                return response;
            }
            if (db.Users.FirstOrDefault(x => x.UserId == login) != null)
            {
                response.MsgError = "Usuário já cadastrado na base de dados.";
                response.Sucess = false;
                return response;
            }

            var newUser = db.Users.Add(new Usuario { RoleId = (int)tipoUsuario, Password = senha, UserId = login, UserName = nomeUser });
            db.SaveChanges();
            response.MsgError = string.Empty;
            response.Sucess = true;
            response.IdUser = newUser.Id;

            return response;
        }
    }
}