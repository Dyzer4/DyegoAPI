using Azure.Core;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Localization;
using Microsoft.Graph;
using System.Text.RegularExpressions;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Interfaces.Services;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;
using TIVIT.CIPA.Api.Domain.Model.Services;
using TIVIT.CIPA.Api.Domain.Providers;
using TIVIT.CIPA.Api.Domain.Resources;
using TIVIT.CIPA.Api.Domain.Settings;
using TIVIT.CIPA.Api.Domain.Validators;


namespace TIVIT.CIPA.Api.Domain.Business
{
    public class SiteBusiness : ISiteBusiness
    {
        private readonly IUserInfo _userInfo;
        private readonly ISiteRepository _siteRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SiteBusiness(
            IUserInfo userInfo,
            ISiteRepository siteRepository,
            IStringLocalizer<SharedResource> localizer)
        {
            _userInfo = userInfo;
            _siteRepository = siteRepository;
            _localizer = localizer;

        }

        public async Task<Response<SiteResponse>> GetByIdAsync(int id)
        {
            var response = new Response<SiteResponse>();

            var site = await this._siteRepository.GetByIdAsync(id);

            if (site == null)
                return response;

            response.Data = new SiteResponse()
            {
                Id = site.Id,
                CompanyId = site.CompanyId,
                Name = site.Name,
                ProtheusCode = site.ProtheusCode,
                IsActive = site.IsActive
            };

            return response;
        }

        public async Task<Response<IEnumerable<SiteResponse>>> GetAllAsync()
        {
            var response = new Response<IEnumerable<SiteResponse>>();

            var sites = await this._siteRepository.GetAllAsync();

            response.Data = sites.Select(x => new SiteResponse()
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                Name = x.Name,
                ProtheusCode = x.ProtheusCode,
                IsActive = x.IsActive
            });

            return response;
        }

        public async Task<Response<IEnumerable<SiteResponse>>> GetActiveAsync()
        {
            var response = new Response<IEnumerable<SiteResponse>>();

            var sites = await this._siteRepository.GetActiveAsync();

            response.Data = sites.Select(x => new SiteResponse()
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                Name = x.Name,
                ProtheusCode = x.ProtheusCode,
                IsActive = x.IsActive
            });

            return response;
        }
    }
}
