using UniMeet.UserModule.Domain.Interests;
using UniMeet.UserModule.Domain.Users;

namespace UniMeet.UserModule.Domain.UserDetails;

public class UserDetail
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Sex Sex { get; set; }
    public List<Interest> Interests { get; set; }
    public string? ProfilePicturePath { get; set; }
    public string? ProfilePictureMimeType { get; set; }

    private UserDetail() { }

    public UserDetail(User user, Sex sex)
    {
        UserId = user.Id;
        User = user;
        Sex = sex;
        Interests = new List<Interest>();
        ProfilePicturePath = null;
        ProfilePictureMimeType = null;
    }
    
    public void AddInterest(Interest interest)
    {
        if (!Interests.Contains(interest))
        {
            Interests.Add(interest);
        }
    }

    public void SetProfilePicture(string picturePath, string mimeType)
    {
        ProfilePicturePath = picturePath;
        ProfilePictureMimeType = mimeType;
    }

    public void RemoveProfilePicture()
    {
        ProfilePicturePath = null;
        ProfilePictureMimeType = null;
    }
}

