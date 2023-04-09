using System.Threading.Tasks;
using Repository;
using RussianMunchkin.Server.DataBase.Models;

namespace RussianMunchkin.Server.DataBase.Command
{
    public class Insert : CommandBase
    {
        private string RegistrationQuery(RegistrationModel model) =>
            $"Insert into users (login, password) values ('{model.Login}', '{model.Password}');";

        public async Task<int> DistributionModels(IModel model)
        {
            return model switch
            {
                RegistrationModel registrationModel 
                    => await PostreSql.ExecuteQuery(RegistrationQuery(registrationModel)),
                _ => 0
            };
        }

        public Insert(PostreSQL postreSql) : base(postreSql)
        {
        }
    }
}