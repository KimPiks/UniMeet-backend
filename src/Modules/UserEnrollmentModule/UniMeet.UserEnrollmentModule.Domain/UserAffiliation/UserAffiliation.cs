namespace UniMeet.UserEnrollmentModule.Domain.UserAffiliation;

public class UserAffiliation
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int FieldOfStudyId { get; set; }
    
    private UserAffiliation() { }
    
    public UserAffiliation(Guid userId, int fieldOfStudyId)
    {
        UserId = userId;
        FieldOfStudyId = fieldOfStudyId;
    }
    
    public void UpdateFieldOfStudy(int fieldOfStudyId)
    {
        FieldOfStudyId = fieldOfStudyId;
    }
}