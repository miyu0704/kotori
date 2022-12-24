using System.Collections;
using System.Collections.Generic;

namespace Useful
{
    /// <summary>
    /// IEnumeratorで参照渡しを使用したいときに使うクラス
    /// </summary>
    public class CanIEnumeratorRefBool
    {
        public bool flag;

        public CanIEnumeratorRefBool()
        {
            flag = false;
        }
    }
}
