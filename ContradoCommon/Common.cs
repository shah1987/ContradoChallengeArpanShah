using System;
using System.Collections.Generic;

namespace ContradoCommon
{
    public static class Common
    {
        private static Dictionary<CustomCodes, string> _res;
        public enum CustomCodes
        {
            /// <summary>
            /// Successfully processed
            /// </summary>
            Success = 1000,
            /// <summary>
            /// Runtime Error
            /// </summary>
            InternalError = 1001,
        }

        public static Dictionary<CustomCodes, string> Response
        {
            get
            {
                if (_res == null)
                {
                    _res = new Dictionary<CustomCodes, string>();
                    _res.Add(CustomCodes.Success, "success");
                    _res.Add(CustomCodes.InternalError, "something went wrong");
                }
                return _res;
            }
                
        
        }
    }
}
