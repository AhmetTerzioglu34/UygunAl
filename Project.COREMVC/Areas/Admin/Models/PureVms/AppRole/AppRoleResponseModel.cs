namespace Project.COREMVC.Areas.Admin.Models.PureVms.AppRole
{
    public class AppRoleResponseModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        //Kullanıcı role sahip mi değil mi
        public bool Checked { get; set; } 
    }
}
