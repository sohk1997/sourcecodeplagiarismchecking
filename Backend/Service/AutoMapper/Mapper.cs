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

            //Document            
            //CreateMap<DocumentUpload, Document>();
            CreateMap<IFormFile, Submission>();
        }
    }
}