using System;
using System.Linq;
using EntityGraphQL;
using EntityGraphQL.Schema;
using CampaignSaber.Models;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;

namespace CampaignSaber.Mutations
{
    public class CampaignMutations
    {
        [GraphQLMutation]
        public Expression<Func<CampaignSaberContext, Campaign>> UpdateCampaign(CampaignSaberContext db, CampaignArgs args, GraphQLValidator validator, IHttpContextAccessor accessor)
        {
            if (string.IsNullOrEmpty(args.Title))
                validator.AddError("Title argument is required");
            if (!string.IsNullOrEmpty(args.Description))
            {
                if (args.Description.Length > 2000)
                    validator.AddError("Description is too long! (Max 2000 characters)");
            }

            var user = accessor.HttpContext.Items["User"];
            if (user == null)
            {
                validator.AddError("Unauthorized Request");
            }

            var cuser = (User)user;

                if (validator.HasErrors)
                return null;

            var campaign = db.Campaigns.FirstOrDefault(c => c.Id == args.Id && (c.UploaderId == cuser.Id || cuser.Role == Role.Admin));
            if (campaign == null)
            {
                validator.AddError("Campaign Not Found");
                return null;
            }
            campaign.Title = args.Title;
            campaign.Description = args.Description;
            db.SaveChanges();
            return ctx => ctx.Campaigns.First(c => c.Id == campaign.Id);
        }

        [GraphQLMutation]
        public Expression<Func<CampaignSaberContext, Campaign>> DeleteCampaign(CampaignSaberContext db, CampaignDeletionArgs args, GraphQLValidator validator, IHttpContextAccessor accessor)
        {
            var user = accessor.HttpContext.Items["User"];
            if (user == null)
            {
                validator.AddError("Unauthorized Request");
            }

            var cuser = (User)user;

            if (validator.HasErrors)
                return null;

            var campaign = db.Campaigns.FirstOrDefault(c => c.Id == args.Id && (c.UploaderId == cuser.Id || cuser.Role == Role.Admin));
            if (campaign == null)
            {
                validator.AddError("Campaign Not Found");
                return null;
            }
            db.Campaigns.Remove(campaign);
            db.SaveChanges();
            return null;
        }
    }
}
