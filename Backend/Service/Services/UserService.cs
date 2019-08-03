using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Root.CommonEnum;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Extension;
using Root.Model;
using ViewModel.Customer;
using ViewModel.ViewModel;

namespace Service.Services
{
    public interface IUserService
    {
        int Create(LoginViewModel cus);
        User GetUser(LoginViewModel model);
    }

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IStoreProcedureRepository _storeProcedure;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper,
            IStoreProcedureRepository storeProcedure)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storeProcedure = storeProcedure;
        }

        public int Create(LoginViewModel cus)
        {
            User user = new User
            {
                IsActive = "true",
                RoleId = (int)RoleType.USER,
                UserName = cus.Username,
                PasswordHash = GetHashString(cus.Password)
            };
            _userRepository.Add(user);
            Commit();
            return user.Id;
        }

        public User GetUser(LoginViewModel model)
        {
            var user = _userRepository.GetAllQueryable()
                                    .Where(u => u.UserName == model.Username && u.PasswordHash == GetHashString(model.Password))
                                    .FirstOrDefault();
            return user;
        }
        private byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}