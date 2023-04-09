using Repository;

namespace RussianMunchkin.Server.DataBase.Command
{
    public class Delete : CommandBase
    {
        public Delete(PostreSQL postreSql) : base(postreSql)
        {
        }
    }
}