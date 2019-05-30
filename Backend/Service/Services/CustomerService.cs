using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AutoMapper;
using Root.Data.Infrastructure;
using Root.Data.Repository;
using Root.Extension;
using Root.Model;
using ViewModel.Customer;

namespace Service.Services {
    public interface ICustomerService {
        List<CustomerInList> CustomerProcedure (int Id, string Name);
        CustomerInfo Get (int Id);
        int Create (CustomerCreate cus);
        void Update (CustomerUpdate cus);
        void Delete (int Id);
    }

    public class CustomerService : ICustomerService {
        private ICustomerRepository _customerRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IStoreProcedureRepository _storeProcedure;

        public CustomerService (ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper,
            IStoreProcedureRepository storeProcedure) {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storeProcedure = storeProcedure;
        }

        public int Create (CustomerCreate cus) {
            Customer customer = _mapper.Map<Customer> (cus);
            _customerRepository.Add (customer);
            Commit ();
            return customer.CustomerId;
        }

        public List<CustomerInList> CustomerProcedure (int Id, string Name) {
            // using (var storeProcedure = _storeProcedure.CallProcedure ("dbo.CustomerProcedure",
            //     Id.ToParam ("CustomerId", DbType.Int32),
            //     Name.ToParam ("Name", DbType.String))) {
            //     var result1 = storeProcedure.GetResultSet<CustomerInList> ();
            //     storeProcedure.NextResult ();
            //     var result2 = storeProcedure.GetResultSet<CustomerInList> ();
            //     return (result1, result2);
            // }
            return Mapper.Map<List<Customer>,List<CustomerInList>>(_customerRepository.GetAllQueryable().ToList());
        }

        public CustomerInfo Get (int Id) {
            var customer = _customerRepository.Get (x => x.CustomerId == Id);
            return _mapper.Map<CustomerInfo> (customer);
        }

        public void Update (CustomerUpdate cus) {
            var customer = _customerRepository.Get (c => c.CustomerId == cus.Id);
            customer = _mapper.Map<CustomerUpdate, Customer> (cus, customer);
            _customerRepository.Update (customer);
            Commit ();
        }

        public void Delete (int Id) {
            _customerRepository.Delete (x => x.CustomerId == Id);
            Commit ();
        }

        private void Commit () {
            _unitOfWork.Commit ();
        }
    }
}