using System;
using Microsoft.IdentityModel.Claims;
using System.Text;
using System.Security.Permissions;

namespace ACSWebApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {           
            // Convert to an IClaimPrincipal so we can access Claims properties
            IClaimsPrincipal claimsPrincipal = (IClaimsPrincipal)this.User;

            // Get collection of claims
            ClaimCollection claims = claimsPrincipal.Identities[0].Claims;
            StringBuilder sbClaims = new StringBuilder();

            // Iterate through claims, build list for rendering to UI
            foreach(Claim claim in claims)
            {
                if (claim.ClaimType.Contains("identityprovider"))
                    lblIdP.Text = claim.Value;
                sbClaims.AppendLine(claim.ClaimType + ": " + claim.Value);
            }
            lblClaims.Text = sbClaims.ToString();

            // check to see if the user is in the admin role
            if(User.IsInRole("admin"))
                EnableAdminAccess();
            
        }

        //Attribute protects method from unauthorized access
        [PrincipalPermission(SecurityAction.Demand, Role="admin")]
        private void EnableAdminAccess()
        {
            lnkAdmin.Visible = true;
        }
    }
}
