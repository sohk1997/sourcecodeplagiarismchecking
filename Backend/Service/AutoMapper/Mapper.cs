using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Root.Model;
using ViewModel.Customer;
using ViewModel.Document;
using ViewModel.ViewModel;
namespace Service.AutoMapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            //Customer
            CreateMap<TestViewModel, TestModel>();
            CreateMap<Customer, CustomerInList>();
            CreateMap<CustomerCreate, Customer>().ForMember(x => x.CustomerName, map => map.MapFrom(y => y.Name));
            CreateMap<CustomerUpdate, Customer>().ForMember(x => x.CustomerName, map => map.MapFrom(y => y.Name))
                .ForMember(x => x.Id, map => map.Ignore());
            CreateMap<Customer, CustomerInfo>().ForMember(x => x.Name, map => map.MapFrom(y => y.CustomerName))
                .ForMember(x => x.Id, map => map.MapFrom(y => y.CustomerId));

            //Document            
            //CreateMap<DocumentUpload, Document>();
            CreateMap<IFormFile, Document>();
            CreateMap<Document, DocumentInList>().ForMember(x => x.Id, map => map.MapFrom(y => y.DocumentId))
                .ForMember(x => x.Name, map => map.MapFrom(y => y.DocumentName))
                .ForMember(x => x.Content, map => map.MapFrom(y => y.DocumentContent))
                .ForMember(x => x.Extn, map => map.MapFrom(y => y.DocumentExtn));
            CreateMap<Document, DocumentInfo>().ForMember(x => x.Id, map => map.MapFrom(y => y.DocumentId))
                .ForMember(x => x.Name, map => map.MapFrom(y => y.DocumentName))
                .ForMember(x => x.Content, map => map.MapFrom(y => y.DocumentContent))
                .ForMember(x => x.Extn, map => map.MapFrom(y => y.DocumentExtn));            
        }
    }
}