using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Root.Model;
using ViewModel.Customer;
using ViewModel.ViewModel;
namespace Service.AutoMapper {
    public class Mapper : Profile {
        public Mapper () {
            CreateMap<TestViewModel, TestModel> ();
            CreateMap<Customer,CustomerInList>();
            CreateMap<CustomerCreate, Customer> ().ForMember (x => x.CustomerName, map => map.MapFrom (y => y.Name));
            CreateMap<CustomerUpdate, Customer> ().ForMember (x => x.CustomerName, map => map.MapFrom (y => y.Name))
                .ForMember (x => x.Id, map => map.Ignore ());
            CreateMap<Customer, CustomerInfo> ().ForMember (x => x.Name, map => map.MapFrom (y => y.CustomerName))
                .ForMember (x => x.Id, map => map.MapFrom (y => y.CustomerId));
        }
    }
}