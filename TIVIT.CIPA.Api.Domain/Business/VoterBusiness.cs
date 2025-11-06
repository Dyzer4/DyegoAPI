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
using TIVIT.CIPA.Api.Domain.Repositories;
using TIVIT.CIPA.Api.Domain.Resources;
using TIVIT.CIPA.Api.Domain.Settings;
using TIVIT.CIPA.Api.Domain.Validators;

namespace TIVIT.CIPA.Api.Domain.Business
{
    public class VoterBusiness : IVoterBusiness
    {
        private readonly IUserInfo _userInfo;
        private readonly IVoterRepository _voterRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public VoterBusiness(
            IUserInfo userInfo,
            IVoterRepository voterRepository,
            IStringLocalizer<SharedResource> localizer)
        {
            _userInfo = userInfo;
            _voterRepository = voterRepository;
            _localizer = localizer;

        }

        public async Task<Response<VoterDetailResponse>> GetByIdAsync(int id)
        {
            var response = new Response<VoterDetailResponse>();

            var voter = await this._voterRepository.GetByIdAsync(id);

            if (voter == null)
                return response;


            response.Data = new VoterDetailResponse()
            {
                Id = voter.Id,
                ElectionId = voter.ElectionId,
                Name = voter.Name,
                Email = voter.Email,
                IsActive = voter.IsActive
            };

            return response;
        }

        public async Task<Response<IEnumerable<VoterResumeResponse>>> GetByElectionIdAsync(int electionId)
        {
            var response = new Response<IEnumerable<VoterResumeResponse>>();

            var voters = await this._voterRepository.GetByElectionIdAsync(electionId);

            if (voters == null)
                return response;

            var photoBase64 =
            response.Data = voters.Select(x => new VoterResumeResponse()
            {
                Id = x.Id,
                ElectionId = x.ElectionId,
                Name = x.Name
            });


            return response;
        }


        public async Task<Response<int>> CreateAsync(VoterCreateRequest createRequest)
        {
            var response = new Response<int>();

            var validator = new VoterValidator(_localizer);
            validator.ValidateCreate(createRequest);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }

            var voter = new Voter()
            {
                ElectionId = createRequest.ElectionId,
                Name = createRequest.Name,
                Email = createRequest.Email,
                IsActive = true,
                CreateDate = DateTime.Now,
                CreateUser = _userInfo.Upn
            };

            await _voterRepository.CreateAsync(voter);

            response.Data = voter.Id;

            return response;
        }

        public async Task<Response> UpdateAsync(int id, VoterUpdateRequest updateRequest)
        {
            var response = new Response();

            var validator = new VoterValidator(_localizer);
            validator.ValidateUpdate(id, updateRequest);
            if (!validator.IsValid)
            {
                response.AddMessage(validator.Notifications.Select(x => x.Message));
                return response;
            }

            var voter = await this._voterRepository.GetByIdAsync(id);

            voter.ElectionId = updateRequest.ElectionId;
            voter.Name = updateRequest.Name;
            voter.Email = updateRequest.Email;
            voter.UpdateDate = DateTime.Now;
            voter.UpdateUser = _userInfo.Upn;

            await _voterRepository.UpdateAsync(voter);

            return response;
        }

        public async Task<Response> ChangeActiveAsync(bool isActive, int id)
        {
            var response = new Response();

            //var validator = new VoterValidator(_localizer);
            //validator.ValidateChangeActive(id);
            //if (!validator.IsValid)
            //{
            //    response.AddMessage(validator.Notifications.Select(x => x.Message));
            //    return response;
            //}

            var voter = await this._voterRepository.GetByIdAsync(id);

            voter.IsActive = isActive;
            voter.UpdateDate = DateTime.Now;
            voter.UpdateUser = _userInfo.Upn;

            await this._voterRepository.UpdateAsync(voter);

            return response;
        }

        public Response<byte[]> GetSyncTemplate()
        {
            var response = new Response<byte[]>();

            var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates\\Example", "sync-voters-template.xlsx");

            using (var workbook = new XLWorkbook(filepath))
            {
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    response.Data = stream.ToArray();
                }
            }

            return response;
        }

        public async Task<Response> SyncByFileAsync(MemoryStream syncFile, int electionId)
        {
            var response = new Response();

            var voters = new List<Voter>();
            using (var workbook = new XLWorkbook(syncFile))
            {
                var worksheet = workbook.Worksheet(1); // Obtém a primeira aba
                int rowCount = worksheet.LastRowUsed().RowNumber(); // Número total de linhas

                for (int row = 2; row <= rowCount; row++) // Começa na linha 2 para ignorar o cabeçalho
                {
                    // Verifica se a linha está vazia (exemplo usando a coluna 'Name' como critério de verificação)
                    if (string.IsNullOrWhiteSpace(worksheet.Cell(row, 1).GetString()))
                    {
                        continue; // Pula a linha vazia
                    }

                    var user = new Voter()
                    {
                        ElectionId = electionId,
                        Name = worksheet.Cell(row, 1).GetString(),
                        Email = worksheet.Cell(row, 2).GetString(),
                        IsActive = true,
                        CreateDate = DateTime.Now,
                        CreateUser = _userInfo.Upn,
                        UpdateDate = DateTime.Now,
                        UpdateUser = _userInfo.Upn
                    };

                    voters.Add(user);
                }
            }
            // após carregar os eleitores, faz as validações, e carrega no bd

            return response;
        }


    }
}
