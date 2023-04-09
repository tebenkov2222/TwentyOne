using System.Threading.Tasks;
using Repository;
using RussianMunchkin.Server.DataBase.Models;

namespace RussianMunchkin.Server.DataBase.Command
{
    public class Update : CommandBase
    {
        private string ModelOne(AuthorizationModel model) => 
            $"update t SET number_test = {model.UserId}, str = '{model.Login}' where id = {model.Password}";
        
        
        
        public async Task<int> UserModel(IModel model)
        {
            return model switch
            {
                AuthorizationModel userModel => await PostreSql.ExecuteQuery(ModelOne(userModel)),
                _ => 0
            };
        }

        public Update(Repository.PostreSQL postreSql) : base(postreSql)
        {
        }
    }
}