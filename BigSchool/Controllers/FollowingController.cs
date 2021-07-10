using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BigSchool.Models;
using Microsoft.AspNet.Identity;
namespace BigSchool.Controllers
{
    public class FollowingController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following follow)
        {
            //user login là ng theo doi, follow.FolloweeId la ng dc theo doix
            var userID = User.Identity.GetUserId();
            if (userID == null)
                return BadRequest("Vui Long dang nhap truoc");
            if (userID == follow.FolloweeId)
                return BadRequest("ok ban da theo gioi bản thân của bạn ((:");
            BigSchoolContext1 context = new BigSchoolContext1();
            // kiem tra xem mã userID đã dc theo doix chưa
            Following find = context.Followings.FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId);
            if (find != null)
            {
                //return BadRequest("The already following exists!!");
                context.Followings.Remove(context.Followings.SingleOrDefault(p => p.FollowerId == userID && p.FolloweeId == follow.FolloweeId));
                context.SaveChanges();
                return Ok("cancel");
            }
            // set object follow
            follow.FollowerId = userID;
            context.Followings.Add(follow);
            context.SaveChanges();
            return Ok();
        }
    }
}
