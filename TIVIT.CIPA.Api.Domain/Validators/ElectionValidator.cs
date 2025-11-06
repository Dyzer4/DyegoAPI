using Flunt.Notifications;
using Flunt.Validations;
using Microsoft.Extensions.Localization;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Repositories;
using TIVIT.CIPA.Api.Domain.Resources;

namespace TIVIT.CIPA.Api.Domain.Validators
{
    public class ElectionValidator : Notifiable<Notification>
    {
        private readonly IElectionRepository _electionRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;


        public ElectionValidator(
            IElectionRepository electionRepository,
            IStringLocalizer<SharedResource> localizer)
        {
            _electionRepository = electionRepository;
            _localizer = localizer;
        }

        public void ValidateCreate(ElectionCreateRequest request)
        {
            var nameExists = ExistsActiveByName(request.Name, null);

            AddNotifications(new Contract<ElectionCreateRequest>()
                .IsNotNullOrWhiteSpace(request.Name, "ElectionInvalidName", "Nome da Eleição inválido")
                .IsTrue(!nameExists, "ExistElectionName", "Nome da Eleição já existe")
                .IsNotNull(request.SitesIds, "SiteNull", "Site deve ser informado")
                .IsGreaterThan(request.SitesIds?.Count ?? 0, 0, "EmptySite", "Site deve ser informado")
            );
        }

        public void ValidateUpdate(int id, ElectionUpdateRequest request)
        {
            var nameExists = ExistsActiveByName(request.Name, id);

            AddNotifications(new Contract<ElectionUpdateRequest>()
                .IsNotNullOrWhiteSpace(request.Name, "ElectionInvalidName", "Nome da Eleição inválido")
                .IsTrue(!nameExists, "ExistElectionName", "Nome da Eleição já existe")
                 .IsNotNull(request.SitesIds, "SiteNull", "Site deve ser informado")
                .IsGreaterThan(request.SitesIds?.Count ?? 0, 0, "EmptySite", "Site deve ser informado")
            );
        }

        public void ValidateChangeActive(int id)
        {
            var electionExists = _electionRepository.ExistsById(id);

            AddNotifications(new Contract<ElectionCreateRequest>()
                .IsTrue(electionExists, "ExistElection", "Eleição não existe")
            );
        }

        public bool ExistsActiveByName(string name, int? ignoreId = null) => _electionRepository.ExistsActiveByName(name, ignoreId);


    }
}
