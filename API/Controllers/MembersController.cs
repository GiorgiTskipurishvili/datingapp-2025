using API.Data;
using API.DTOs;
using API.Entities;
using API.Extension;
using API.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Authorize]
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>> >GetMembers()
        {
            return Ok(await memberRepository.GetMembersAsync());
        }

        [HttpGet("{id}")] //localhost:5000/api/members/bob-id   
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);
            if (member == null) return NotFound();
            return member;
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await memberRepository.GetPhotosByMemberIdAsync(id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdatedDto memberUpdatedDto)
        {
            var memberId = User.GetMemberId();

            var member = await memberRepository.GetMemberForUpdate(memberId);

            if(member == null) return BadRequest("Could not get member");
            
            member.DisplayName = memberUpdatedDto.DisplayName ?? member.DisplayName;
            member.Description = memberUpdatedDto.Description ?? member.Description;
            member.City = memberUpdatedDto.City ?? member.City;
            member.Country = memberUpdatedDto.Country ?? member.Country;

            member.User.DisplayName = memberUpdatedDto.DisplayName ?? member.User.DisplayName;


            memberRepository.Update(member); //optional

            if (await memberRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update member");

        }

    }
}
