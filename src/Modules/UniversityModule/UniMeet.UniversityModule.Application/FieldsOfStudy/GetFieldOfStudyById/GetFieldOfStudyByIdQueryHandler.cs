using UniMeet.Shared.Abstractions;
using UniMeet.UniversityModule.Domain.Universities;

namespace UniMeet.UniversityModule.Application.FieldsOfStudy.GetFieldOfStudyById;

public class GetFieldOfStudyByIdQueryHandler(IUniversityRepository universityRepository)
    : IQueryHandler<GetFieldOfStudyByIdQuery, FieldOfStudyDto?>
{
    public async Task<FieldOfStudyDto?> HandleAsync(GetFieldOfStudyByIdQuery request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var university = await universityRepository.GetByFieldOfStudyIdAsync(request.FieldOfStudyId, cancellationToken);
        if (university == null)
            throw new KeyNotFoundException("University not found");

        var fieldOfStudy = university.GetFieldOfStudyById(request.FieldOfStudyId);
        if (fieldOfStudy == null)
            return null;

        return new FieldOfStudyDto
        {
            Id = fieldOfStudy.Id,
            Name = fieldOfStudy.Name
        };
    }
}