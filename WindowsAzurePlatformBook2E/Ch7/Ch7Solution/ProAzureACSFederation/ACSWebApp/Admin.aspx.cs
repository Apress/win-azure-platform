using System;
using Microsoft.IdentityModel.Claims;
using System.Text;
using System.Security.Permissions;

namespace ACSWebApp
{
    public partial class Admin : System.Web.UI.Page
    {
        // Attribute protects against unauthorized access.
        // Whether user is authenticated via claims-based security or some other method, they must belong to the "admin" role.
        // In this example, the role access was presented as a claim in the token from ACS
        [PrincipalPermission(SecurityAction.Demand, Role="admin")]
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}