using System.Collections.Generic;
using ApplicationCore.Models;
using Infrastructure.Models;


namespace ApplicationCore.Services.Actions
{


    public interface IActionService
    {
        public List<Action> GetActions();
        public Action GetActions(System.Guid id);
        public int CreateAction(ActionParam actionParam);
        public int UpdateAction(System.Guid id, ActionParam actionParam);
        public int DeleteAction(System.Guid id);
        public int CountAction();
    }
}
