using System.Collections.Generic;
using UniMeet.Shared.Abstractions;

namespace ModularSystem.Contracts.Matching.Likes.GetLikes;

public record GetLikesQuery(Guid UserId) : IQuery<IEnumerable<Guid>>;


