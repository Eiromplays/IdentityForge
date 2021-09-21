using System;
using Eiromplays.IdentityServer.Application.Common.Interface;

namespace Eiromplays.IdentityServer.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
