using DataPostAPI.Helpers;
using DataPostAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataPostAPI.Services
{
    public interface IClientService
    {
        Client Authenticate(string username, string password);
        IEnumerable<Client> GetAll();
        Client GetById(int id);
        Client Create(Client client, string password);
        void Update(Client client, string password = null);
        void Delete(int id);
    }

    public class ClientService : IClientService
    {
        private ClientContext _context;

        public ClientService(ClientContext context)
        {
            _context = context;
        }

        public Client Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var client = _context.Client.SingleOrDefault(x => x.ClientName == username);

            // check if username exists
            if (client == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, client.PasswordHash, client.PasswordSalt))
                return null;

            // authentication successful
            return client;
        }

        public IEnumerable<Client> GetAll()
        {
            return _context.Client;
        }

        public Client GetById(int id)
        {
            return _context.Client.Find(id);
        }

        public Client Create(Client client, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (string.IsNullOrWhiteSpace(client.ZoneId.ToString()))
                throw new AppException("Zone number is required");

            if (_context.Client.Any(x => x.ClientName == client.ClientName))
                throw new AppException("Username \"" + client.ClientName + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            client.PasswordHash = passwordHash;
            client.PasswordSalt = passwordSalt;

            _context.Client.Add(client);
            _context.SaveChanges();

            return client;
        }

        public void Update(Client userParam, string password = null)
        {
            var client = _context.Client.Find(userParam.ClientId);

            if (client == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.ClientName) && userParam.ClientName != client.ClientName)
            {
                // throw error if the new username is already taken
                if (_context.Client.Any(x => x.ClientName == userParam.ClientName))
                    throw new AppException("Username " + userParam.ClientName + " is already taken");

                client.ClientName = userParam.ClientName;
            }

            // update client properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                client.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                client.LastName = userParam.LastName;

            if (!string.IsNullOrWhiteSpace(userParam.ZoneId.ToString()))
                client.ZoneId = userParam.ZoneId;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                client.PasswordHash = passwordHash;
                client.PasswordSalt = passwordSalt;
            }

            _context.Client.Update(client);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var client = _context.Client.Find(id);
            if (client != null)
            {
                _context.Client.Remove(client);
                _context.SaveChanges();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
