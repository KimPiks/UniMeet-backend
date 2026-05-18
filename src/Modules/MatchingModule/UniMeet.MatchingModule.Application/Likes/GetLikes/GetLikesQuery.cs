using System.Collections.Generic;
using UniMeet.Shared.Abstractions;

namespace UniMeet.MatchingModule.Application.Likes.GetLikes;

public record GetLikesQuery(Guid UserId) : IQuery<IEnumerable<Guid>>;


