using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.Strategy
{
    public class StrategyFactory
    {

        Dictionary<HandleForm.SaveType, IHandleDataStrategy> dic = new Dictionary<HandleForm.SaveType, IHandleDataStrategy> ();
        private static StrategyFactory instance = null;

        private StrategyFactory() {
            RegisterWith(HandleForm.SaveType.Insert, new HandleInsert());
            RegisterWith(HandleForm.SaveType.Update, new HandleUpdate());
        }

        public static StrategyFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new StrategyFactory();
            }
            return instance;
        }

        private void RegisterWith(HandleForm.SaveType a, IHandleDataStrategy s)
        {
            dic.Add(a, s);
        }

        public IHandleDataStrategy GetStrategy(HandleForm.SaveType a)
        {
            return dic[a];
        }
        
    }
}
