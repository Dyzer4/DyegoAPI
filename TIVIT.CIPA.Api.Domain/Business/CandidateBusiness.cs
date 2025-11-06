using Azure.Core;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Localization;
using Microsoft.Graph;
using System.Security.Policy;
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
    public class CandidateBusiness : ICandidateBusiness
    {
        private readonly IUserInfo _userInfo;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CandidateBusiness(
            IUserInfo userInfo,
            ICandidateRepository candidateRepository,
            IStringLocalizer<SharedResource> localizer)
        {
            _userInfo = userInfo;
            _candidateRepository = candidateRepository;
            _localizer = localizer;

        }

        public async Task<Response<CandidateDetailResponse>> GetByIdAsync(int id)
        {
            var response = new Response<CandidateDetailResponse>();

            var candidate = await this._candidateRepository.GetByIdAsync(id);

            if (candidate == null)
                return response;

            var photoBase64 = $"data:{candidate.PhotoMimeType};base64,{Convert.ToBase64String(candidate.PhotoBase64)}";

            response.Data = new CandidateDetailResponse()
            {
                Id = candidate.Id,
                ElectionId = candidate.ElectionId,
                Name = candidate.Name,
                Area = candidate.Area,
                SiteId = candidate.SiteId,
                PhotoBase64 = photoBase64,
                IsActive = candidate.IsActive,
                Site = candidate.Site != null? new SiteResponse()
                {
                    Id = candidate.Site.Id,
                    CompanyId = candidate.Site.CompanyId,
                    ProtheusCode = candidate.Site.ProtheusCode,
                    IsActive = candidate.Site.IsActive,
                    Name = candidate.Site.Name
                }:null
            };

            return response;
        }

        public async Task<Response<IEnumerable<CandidateDetailResponse>>> GetByElectionIdAsync(int electionId)
        {
            var response = new Response<IEnumerable<CandidateDetailResponse>>();

            var candidates = await this._candidateRepository.GetByElectionIdAsync(electionId);

            if (candidates == null)
                return response;

            response.Data = candidates.Select(x => new CandidateDetailResponse()
            {
                Id = x.Id,
                ElectionId = x.ElectionId,
                Name = x.Name,
                Area = x.Area,
                SiteId = x.SiteId,
                PhotoBase64 = $"data:{x.PhotoMimeType};base64,{Convert.ToBase64String(x.PhotoBase64)}",
                IsActive = x.IsActive,
                Site = x.Site != null ? new SiteResponse()
                {
                    Id = x.Site.Id,
                    CompanyId = x.Site.CompanyId,
                    ProtheusCode = x.Site.ProtheusCode,
                    IsActive = x.Site.IsActive,
                    Name = x.Site.Name
                } : null
            });


            return response;
        }

        public async Task<Response<IEnumerable<CandidateResumeResponse>>> SearchCandidateAsync(string name, int? electionId, bool? isActive, int? siteId)
        {
            var response = new Response<IEnumerable<CandidateResumeResponse>>();

            var candidates = await this._candidateRepository.SearchAsync(name, electionId, isActive, siteId);

            if (candidates == null)
                return response;

            response.Data = candidates.Select(x => new CandidateResumeResponse()
            {
                Id = x.Id,
                ElectionId = x.ElectionId,
                CorporateId = x.CorporateId,
                Name = x.Name,
                Area = x.Area,
                IsActive = x.IsActive
            });


            return response;
        }


        public async Task<Response<int>> CreateAsync(CandidateCreateRequest createRequest)
        {
            var response = new Response<int>();

            byte[] photoBytes = null;
            string mimeType = null;
            string base64Data = null;
            try
            {


                if (!string.IsNullOrEmpty(createRequest.PhotoBase64))
                {
                    base64Data = createRequest.PhotoBase64;
                    var base64Index = base64Data.IndexOf("base64,");
                    if (base64Index >= 0)
                    {
                        var match = Regex.Match(createRequest.PhotoBase64, @"^data:(?<mime>.+?);base64,(?<data>.+)$");
                        if (match.Success)
                        {
                            mimeType = match.Groups["mime"].Value;
                            photoBytes = Convert.FromBase64String(match.Groups["data"].Value);
                        }
                    }
                    else
                    {
                        photoBytes = Convert.FromBase64String(base64Data);
                        mimeType = GetImageMimeType(photoBytes);
                    }
                }

            }
            catch (Exception)
            {
                throw new ArgumentException("Formato de imagem (Base64) inválido.");
            }


            var validator = new CandidateValidator(_candidateRepository, _localizer);
            validator.ValidateCreate(createRequest, photoBytes, mimeType);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }




            var candiate = new Candidate()
            {
                ElectionId = createRequest.ElectionId,
                CorporateId = createRequest.CorporateId,
                Name = createRequest.Name,
                Area = createRequest.Area,
                SiteId = createRequest.SiteId,
                PhotoBase64 = photoBytes,
                PhotoMimeType = mimeType,
                IsActive = true,
                CreateDate = DateTime.Now,
                CreateUser = _userInfo.Upn
            };

            await _candidateRepository.CreateAsync(candiate);

            response.Data = candiate.Id;

            return response;
        }

        public async Task<Response> UpdateAsync(int id, CandidateUpdateRequest updateRequest)
        {
            var response = new Response();

            byte[] photoBytes = null;
            string mimeType = null;
            string base64Data = null;
            try
            {


                if (!string.IsNullOrEmpty(updateRequest.PhotoBase64))
                {
                    base64Data = updateRequest.PhotoBase64;
                    var base64Index = base64Data.IndexOf("base64,");
                    if (base64Index >= 0)
                    {
                        var match = Regex.Match(updateRequest.PhotoBase64, @"^data:(?<mime>.+?);base64,(?<data>.+)$");
                        if (match.Success)
                        {
                            mimeType = match.Groups["mime"].Value;
                            photoBytes = Convert.FromBase64String(match.Groups["data"].Value);
                        }
                    }
                    else
                    {
                        photoBytes = Convert.FromBase64String(base64Data);
                        mimeType = GetImageMimeType(photoBytes);
                    }
                }

            }
            catch (Exception)
            {
                throw new ArgumentException("Formato de imagem (Base64) inválido.");
            }

            var validator = new CandidateValidator(_candidateRepository, _localizer);
            validator.ValidateUpdate(id, updateRequest, photoBytes, mimeType);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }

            var candidate = await this._candidateRepository.GetByIdAsync(id);

            candidate.ElectionId = updateRequest.ElectionId;
            candidate.CorporateId = updateRequest.CorporateId;
            candidate.Name = updateRequest.Name;
            candidate.Area = updateRequest.Area;
            candidate.SiteId = updateRequest.SiteId;
            candidate.PhotoBase64 = photoBytes;
            candidate.PhotoMimeType = mimeType;
            candidate.UpdateDate = DateTime.Now;
            candidate.UpdateUser = _userInfo.Upn;

            await _candidateRepository.UpdateAsync(candidate);

            return response;
        }

        public async Task<Response> ChangeActiveAsync(bool isActive, int id)
        {
            var response = new Response();

            var validator = new CandidateValidator(_candidateRepository, _localizer);
            validator.ValidateChangeActive(id);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }

            var candidate = await this._candidateRepository.GetByIdAsync(id);

            candidate.IsActive = isActive;
            candidate.UpdateDate = DateTime.Now;
            candidate.UpdateUser = _userInfo.Upn;

            await this._candidateRepository.UpdateAsync(candidate);

            return response;
        }

        #region "Private Methods"
        private string GetImageMimeType(byte[] imageBytes)
        {
            if (imageBytes.Length < 4)
                return "application/octet-stream";

            // PNG: 89 50 4E 47
            if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
                return "image/png";

            // JPEG: FF D8 FF
            if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
                return "image/jpeg";

            // GIF: 47 49 46 38
            if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x38)
                return "image/gif";

            // BMP: 42 4D
            if (imageBytes[0] == 0x42 && imageBytes[1] == 0x4D)
                return "image/bmp";

            return "application/octet-stream"; // tipo genérico
        }
        #endregion
    }
}
