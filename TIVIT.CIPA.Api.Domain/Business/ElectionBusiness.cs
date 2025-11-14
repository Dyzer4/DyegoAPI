using Azure.Core;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Localization;
using Microsoft.Graph;
using TIVIT.CIPA.Api.Domain.Interfaces.Business;
using TIVIT.CIPA.Api.Domain.Interfaces.Models;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Interfaces.Services;
using TIVIT.CIPA.Api.Domain.Model;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Model.Responses;
using TIVIT.CIPA.Api.Domain.Model.Services;
using TIVIT.CIPA.Api.Domain.Providers;
using TIVIT.CIPA.Api.Domain.Repositories;
using TIVIT.CIPA.Api.Domain.Resources;
using TIVIT.CIPA.Api.Domain.Settings;
using TIVIT.CIPA.Api.Domain.Validators;

namespace TIVIT.CIPA.Api.Domain.Business
{
    public class ElectionBusiness : IElectionBusiness
    {
        private readonly IUserInfo _userInfo;
        private readonly IElectionRepository _electionRepository;
        private readonly IElectionSiteRepository _electionSiteRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ElectionBusiness(
            IUserInfo userInfo,
            IElectionRepository electionRepository,
            IElectionSiteRepository electionSiteRepository,
            IStringLocalizer<SharedResource> localizer)
        {
            _userInfo = userInfo;
            _electionRepository = electionRepository;
            _electionSiteRepository = electionSiteRepository;
            _localizer = localizer;
        }

        public async Task<Response<ElectionResponse>> GetByIdAsync(int id)
        {
            var response = new Response<ElectionResponse>();

            var election = await this._electionRepository.GetByIdAsync(id);

            if (election == null)
                return response;

            var sites = await _electionSiteRepository.GetSitesByElectionIdAsync(id);

            response.Data = new ElectionResponse()
            {
                Id = election.Id,
                Code = election.Code,
                Name = election.Name,
                Description = election.Description,
                StartDate = election.StartDate,
                EndDate = election.EndDate,
                RegistrationStartDate = election.RegistrationStartDate,
                RegistrationEndDate = election.RegistrationEndDate,
                ElectionStartDate = election.ElectionStartDate,
                ElectionEndDate = election.ElectionEndDate,
                Type = election.Type,
                IsActive = election.IsActive,
                Sites = sites.Select(site => new SiteResponse()
                {
                    Id = site.Id,
                    CompanyId = site.CompanyId,
                    ProtheusCode = site.ProtheusCode,
                    IsActive = site.IsActive,
                    Name = site.Name
                })
            };

            return response;
        }

        public async Task<Response<int>> CreateAsync(ElectionCreateRequest createRequest)
        {
            var response = new Response<int>();

            var validator = new ElectionValidator(_electionRepository, _localizer);
            validator.ValidateCreate(createRequest);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }

            var election = new Election()
            {
                CompanyId = createRequest.CompanyId,
                Code = createRequest.Code,
                Name = createRequest.Name,
                Description = createRequest.Description,
                StartDate = createRequest.StartDate,
                EndDate = createRequest.EndDate,
                RegistrationStartDate = createRequest.RegistrationStartDate,
                RegistrationEndDate = createRequest.RegistrationEndDate,
                ElectionStartDate = createRequest.ElectionStartDate,
                ElectionEndDate = createRequest.ElectionEndDate,
                Type = createRequest.Type ?? "Regular",
                InvitationMessage = createRequest.InvitationMessage,
                IsActive = true,
                CreateDate = DateTime.Now,
                CreateUser = _userInfo.Upn
            };

            await _electionRepository.CreateAsync(election);

            #region "Sites da Eleição"
            if (createRequest.SitesIds != null && createRequest.SitesIds.Any())
            {
                var newSites = createRequest.SitesIds.Select(siteId => new ElectionSite()
                {
                    SiteId = siteId,
                    ElectionId = election.Id
                });

                await _electionSiteRepository.AddRangeAsync(newSites);
            }
            #endregion

            response.Data = election.Id;

            return response;
        }

        public async Task<Response> UpdateAsync(int id, ElectionUpdateRequest updateRequest)
        {
            var response = new Response();

            var validator = new ElectionValidator(_electionRepository, _localizer);
            validator.ValidateUpdate(id, updateRequest);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }

            var election = await this._electionRepository.GetByIdAsync(id);

            if (election == null)
            {
                response.AddMessage("Eleição não encontrada.");
                return response;
            }

            election.Code = updateRequest.Code ?? election.Code;
            election.Name = updateRequest.Name ?? election.Name;
            election.Description = updateRequest.Description ?? election.Description;
            election.StartDate = updateRequest.StartDate ?? election.StartDate;
            election.EndDate = updateRequest.EndDate ?? election.EndDate;
            election.RegistrationStartDate = updateRequest.RegistrationStartDate ?? election.RegistrationStartDate;
            election.RegistrationEndDate = updateRequest.RegistrationEndDate ?? election.RegistrationEndDate;
            election.ElectionStartDate = updateRequest.ElectionStartDate ?? election.ElectionStartDate;
            election.ElectionEndDate = updateRequest.ElectionEndDate ?? election.ElectionEndDate;
            election.Type = updateRequest.Type ?? election.Type;
            election.InvitationMessage = updateRequest.InvitationMessage ?? election.InvitationMessage;
            election.UpdateDate = DateTime.Now;
            election.UpdateUser = _userInfo.Upn;

            await _electionRepository.UpdateAsync(election);

            #region "Sites da Eleição"
            if (updateRequest.SitesIds != null && updateRequest.SitesIds.Any())
            {
                var oldSites = await _electionSiteRepository.GetByElectionId(id);

                if (oldSites != null && oldSites.Any())
                {
                    await _electionSiteRepository.RemoveRangeAsync(oldSites);
                }

                var newSites = updateRequest.SitesIds.Select(siteId => new ElectionSite()
                {
                    SiteId = siteId,
                    ElectionId = id
                });

                await _electionSiteRepository.AddRangeAsync(newSites);
            }
            #endregion

            return response;
        }

        public async Task<Response> ChangeActiveAsync(bool isActive, int id)
        {
            var response = new Response();

            var validator = new ElectionValidator(_electionRepository, _localizer);
            validator.ValidateChangeActive(id);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }

            var election = await this._electionRepository.GetByIdAsync(id);

            if (election == null)
            {
                response.AddMessage("Eleição não encontrada.");
                return response;
            }

            election.IsActive = isActive;
            election.UpdateDate = DateTime.Now;
            election.UpdateUser = _userInfo.Upn;

            await this._electionRepository.UpdateAsync(election);

            return response;
        }
    }
}
