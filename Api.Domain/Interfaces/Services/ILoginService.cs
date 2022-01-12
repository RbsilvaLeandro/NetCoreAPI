using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.DTO;
using Api.Domain.Entities;

namespace Api.Domain.Interfaces.Services
{
    public interface ILoginService
    {
        Task<object> FindByLogin(LoginDto user);
    }
}
