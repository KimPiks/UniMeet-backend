using UniMeet.UniversityModule.Application.DTOs;
using UniMeet.UniversityModule.Application.Mappers;
using UniMeet.UniversityModule.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniMeet.Shared.Abstractions;

namespace UniMeet.UniversityModule.Application.Universities.Queries.GetAllUniversities;
public class GetAllUniversitiesQueryHandler : IRequestHandler<GetAllUniversitiesQuery, IEnumerable<UniversityDto>>
    {
        private readonly IUniversityRepository _universityRepository;
        public GetAllUniversitiesQueryHandler(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        public async Task<IEnumerable<UniversityDto>> HandleAsync(GetAllUniversitiesQuery request, CancellationToken cancellationToken)
        {
            var universities = await _universityRepository.GetAllAsync();
            return universities.Select(university => university.ToDto()).ToList();
        }
    }