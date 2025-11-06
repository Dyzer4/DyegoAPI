using Flunt.Notifications;
using Flunt.Validations;
using Microsoft.Extensions.Localization;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Repositories;
using TIVIT.CIPA.Api.Domain.Resources;

namespace TIVIT.CIPA.Api.Domain.Validators
{
    public class VoterValidator : Notifiable<Notification>
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        public VoterValidator(
            IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public void ValidateCreate(VoterCreateRequest request)
        {

            AddNotifications(new Contract<VoterCreateRequest>()
                .IsNotNullOrWhiteSpace(request.Name, "Name", "Nome do candidato é obrigatório")
            );

        }

        public void ValidateUpdate(int id, VoterUpdateRequest request)
        {
            AddNotifications(new Contract<CandidateUpdateRequest>()
                .IsNotNullOrWhiteSpace(request.Name, "Name", "Nome do candidato é obrigatório")               
            );


        }


    }
}
