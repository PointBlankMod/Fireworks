using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;
using PointBlank.API;
using PointBlank.API.Collections;

namespace Rocket.Core.Assets
{
    public class ClassToListAsset<C, L> : Asset<C> where C : class, IRocketPluginConfiguration where L : ConfigurationList
    {
        #region Variables
        private L _List;
        #endregion

        public ClassToListAsset(C _class, L list)
        {
            // Set the variables
            _Instance = _class;
            _List = list;

            // Run the code
            _Instance.LoadDefaults();

            foreach (FieldInfo fi in typeof(C).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                if (!_List.ContainsKey(fi.Name) && !fi.Name.EndsWith(">k__BackingField"))
                    _List.Add(fi.Name, fi.GetValue(_Instance));
            foreach(PropertyInfo pi in typeof(C).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                if (!_List.ContainsKey(pi.Name))
                    _List.Add(pi.Name, pi.GetValue(_Instance, null));
        }

        #region Functions
        public override void Load(AssetLoaded<C> callback = null)
        {
            foreach(FieldInfo fi in typeof(C).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                if (_List.ContainsKey(fi.Name) && !fi.Name.EndsWith(">k__BackingField"))
                    fi.SetValue(_Instance, _List[fi.Name]);
            foreach (PropertyInfo pi in typeof(C).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                if (_List.ContainsKey(pi.Name))
                    pi.SetValue(_Instance, _List[pi.Name], null);

            base.Load(callback);
        }
        #endregion
    }
}
