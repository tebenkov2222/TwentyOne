using System.Threading.Tasks;
using Repository;
using RussianMunchkin.Server.DataBase.Models;

namespace RussianMunchkin.Server.DataBase.Command
{
    public class Select : CommandBase
    {
        public async Task<AuthorizationModel> GetAuthorizationModel(AuthorizationModel authorizationModel) =>
            await PostreSql.GetModel<AuthorizationModel>($"Select * From users where '{authorizationModel.Login}' = login");
        
        public async Task<RegistrationModel> GetRegistrationModel(RegistrationModel registrationModel) =>
            await PostreSql.GetModel<RegistrationModel>($"Select * From users where '{registrationModel.Login}' = login");

        public async Task<CheckLoginModel> CheckLogin(CheckLoginModel checkLoginModel) =>
            await PostreSql.GetModel<CheckLoginModel>(
                $"select count(*) <> 0 as login_exists from users where login = '{checkLoginModel.Login}';");
        
        public Select(PostreSQL postreSql) : base(postreSql)
        {
        }
    }
}