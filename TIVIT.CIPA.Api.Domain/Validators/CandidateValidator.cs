using Flunt.Notifications;
using Flunt.Validations;
using Microsoft.Extensions.Localization;
using TIVIT.CIPA.Api.Domain.Interfaces.Repositories;
using TIVIT.CIPA.Api.Domain.Model.Requests;
using TIVIT.CIPA.Api.Domain.Repositories;
using TIVIT.CIPA.Api.Domain.Resources;

namespace TIVIT.CIPA.Api.Domain.Validators
{
    public class CandidateValidator : Notifiable<Notification>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        private const int MaxImageSizeBytes = 3 * 1024 * 1024; // 3 MB

        private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/jpg", "image/png" };


        public CandidateValidator(
            ICandidateRepository candidateRepository,
            IStringLocalizer<SharedResource> localizer)
        {
            _candidateRepository = candidateRepository;
            _localizer = localizer;
        }

        public void ValidateCreate(CandidateCreateRequest request, byte[] photoBytes, string mimeType)
        {

            AddNotifications(new Contract<CandidateCreateRequest>()
                .Requires()

                .IsNotNullOrWhiteSpace(request.CorporateId, "CorporateId", "Matricula é obrigatório")
                .IsGreaterThan(request.ElectionId, 0, "ElectionId", "ElectionId inválido")
                .IsNotNullOrWhiteSpace(request.PhotoBase64, "Foto", "Foto do candidato é obrigatório")
            );

            if (string.IsNullOrWhiteSpace(request.PhotoBase64))
                return;

            if (string.IsNullOrWhiteSpace(mimeType) || !AllowedMimeTypes.Contains(mimeType))
            {
                AddNotification(nameof(request.PhotoBase64), "Tipo de imagem inválido. Use JPEG ou PNG.");
                return;
            }

            // Valida Base64 e tamanho
            if (photoBytes.Length > MaxImageSizeBytes)
                AddNotification(nameof(request.PhotoBase64), "A imagem não pode ultrapassar 3 MB.");


        }

        public void ValidateUpdate(int id, CandidateUpdateRequest request, byte[] photoBytes, string mimeType)
        {
            AddNotifications(new Contract<CandidateUpdateRequest>()
                .Requires()

                .IsNotNullOrWhiteSpace(request.Name, "Name", "Nome do candidato é obrigatório")
                .IsNotNullOrWhiteSpace(request.CorporateId, "CorporateId", "Matricula é obrigatório")
                .IsGreaterThan(request.ElectionId, 0, "ElectionId", "ElectionId inválido")
                .IsNotNullOrWhiteSpace(request.PhotoBase64, "Foto", "Foto do candidato é obrigatório")
            );

            if (string.IsNullOrWhiteSpace(request.PhotoBase64))
                return;

            if (string.IsNullOrWhiteSpace(mimeType) || !AllowedMimeTypes.Contains(mimeType))
            {
                AddNotification(nameof(request.PhotoBase64), "Tipo de imagem inválido. Use JPEG ou PNG.");
                return;
            }

            // Valida Base64 e tamanho
            if (photoBytes.Length > MaxImageSizeBytes)
                AddNotification(nameof(request.PhotoBase64), "A imagem não pode ultrapassar 3 MB.");


        }

        public void ValidateChangeActive(int id)
        {
            var candidateExists = _candidateRepository.ExistsById(id);

            AddNotifications(new Contract<CandidateCreateRequest>()
                .IsTrue(candidateExists, "ExistCandidate", "Candidato não existe")
            );
        }

        //public bool ExistsActiveByName(string name, int? ignoreId = null) => _candidateRepository.ExistsActiveByName(name, ignoreId);


    }
}
