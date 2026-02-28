using UniMeet.Shared.Exceptions;

namespace UniMeet.UserEnrollmentModule.Domain.UserAffiliation.Exceptions;

public sealed class AffiliationNotFoundException(int id) 
    : DomainException($"Affiliation with id = {id} was not found.");