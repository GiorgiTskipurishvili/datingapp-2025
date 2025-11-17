using API.Entities;
using API.Helpers;

namespace API.Interface
{
    public interface IMemberRepository
    {
        void Update(Member member);
        Task<PaginatedResult<Member>> GetMembersAsync(MemberParams memberParams);
        Task<Member?> GetMemberByIdAsync(string id);
        Task<IReadOnlyList<Photo>> GetPhotosByMemberIdAsync(string memberId);
        Task<Member?> GetMemberForUpdate(string id);
    }
}
