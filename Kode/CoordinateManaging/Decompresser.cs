using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateManaging
{
    public class Decompresser
    {
        public List<Tuple<double, double>> Start(List<String> compressedGeometry)
        {
            var decompressedGeometryList = new List<Tuple<double, double>>();
            //string compressed = "+34+pcuhk+jirn14+ls+34+fk+io+0+fk-68+cg-cg+cg-fk+34-fk-68-1l4+v8-3a8+1es-1i0+1i0-12c+1es-6b4+dic-v8+27s-s4+438+0+4m0+fk+7dg+1l4+qug+io+86g+68+744-9c+404-p0+4s8-30s+gg4-fk+30s-cg+2kc-s4+4m0-9c+1i0-p0+404-340+ens-27s+808-18k+5f0+0+34-1ug+744-2qk+8j0-27s+7ms-1o8+6b4-1bo+6nk-v8+6e8-cg+98s-68+8fs-s4+ul8-p0+5uk-2to+hoo-1i0+92k-3gg+lfg-io+3jk-68+374+68+2h8+fk+24o+1i0+52g+cg+v8+cg+15g+2qk+808+34+34+2b0+6nk+15g+2ng+12c+21k+ls+12c+8vg+86g+1rc+24o+1ug+340+438+710+7dg+cj4+1o8+2kc+1ug+2e4+4p4+6b4+2e4+3ps+v8+2e4+io+2ng+34+404-68+8co-fk+374-18k+4is-2kc+744-34+34-cg+12c-v8+2qk-18k+3jk-374+bgo-fk+2h8+0+1rc+68+1es+io+1i0+2b0+3a8+4fo+52g+s4+s4+1o8+24o+2h8+3mo+27s+3ps+io+s4+ls+18k+3a8+5i4+2b0+4ck+1o8+55k+ls+3dc+2to+aqs+1bo+5uk+io+4s8+34+680-34+6b4-cg+4p4-1l4+9f4-1es+cpc-fk+5uk-34+680+9c+404+0+27s-ls+4ck-cg+1i0-1ug+808-s4+52g-68+3jk+cg+9f4-34+5rg-68+24o-fk+2kc-io+21k-fk+ls-v8+1o8-c9o+e20-1l4+1ug-5oc+6b4-1bo+1i0-1bo+1es-5oc+6nk-1es+21k-v8+1rc-ls+21k-9c+1i0-34+1l4+34+4s8+2kc+1ars+ls+7ac+p0+83c+1es+j4g+9c+nk8-34+3a8-io+4ck-p0+4ck-1bo+4s8-v8+30s-18k+2ng-12c+1bo-2ng+30s-404+3mo-1l4+1rc-1i0+1bo-2kc+2to-27s+2to-2ng+404-18k+21k-3mo+52g-s4+1i0-p0+p0-2b0+3gg-2qk+4is-61o+cj4-1o8+4is-p0+3a8-fk+2qk-9c+4p4+cg+58o+p0+4ck+340+f18+24o+b48+p0+64s+io+4vc+68+3ps-34+710-3gg+12f4-fk+808+0+4s8+1es+14n0+68+4p4+0+374-9c+4s8-21k+csg-2ng+fq8-5uk+12og-1ug+7ms-2ng+a84-24o+86g-3dc+dic-d2o+1i34-2ng+ahg-21k+680-1rc+4is-21k+4is-6ts+d2o-1es+3a8-27s+6qo-15g+5i4-3ps+nas+0+1rc+9c+1ug+1bo+3gg+cg+s4+15g+30s+v8+2h8+1es+3a8+v8+2kc+3mo+8p8+21k+438+12c+21k+2ng+5oc+1l4+49g+io+27s+io+3jk+34+3gg-io+98s+9c+2b0+fk+v8";
            foreach(var i in compressedGeometry)
            {

                var a = ExtractPointsFromCompressedGeometry(i);
               
                foreach (var k in a)
                    decompressedGeometryList.Add(new Tuple<double, double>(k.x, k.y));
            }

            return decompressedGeometryList;


        }
        private struct XY
        {
            public double x;
            public double y;
        }
        private char[] m_abc = {'0','1','2','3','4','5','6','7','8','9',
            'a','b','c','d','e','f','g','h','i','j',
            'k','l','m','n','o','p','q','r','s','t','u','v'};
        private XY[] ExtractPointsFromCompressedGeometry(System.String compresedGeometry)
        {
            // initialize result storage
            System.Collections.Generic.List<XY> result = new System.Collections.Generic.List<XY>(); // memory exception
            int nIndex = 0;
            double dMultBy = (double)ExtractInt(compresedGeometry, ref nIndex); // exception
            int nLastDiffX = 0;
            int nLastDiffY = 0;
            int nLength = compresedGeometry.Length; // reduce call stack
            while (nIndex != nLength)
            {
                // extract number
                int nDiffX = ExtractInt(compresedGeometry, ref nIndex); // exception
                int nDiffY = ExtractInt(compresedGeometry, ref nIndex); // exception
                // decompress
                int nX = nDiffX + nLastDiffX;
                int nY = nDiffY + nLastDiffY;
                double dX = (double)nX / dMultBy;
                double dY = (double)nY / dMultBy;
                // add result item
                XY point = new XY();
                point.x = dX;
                point.y = dY;
                result.Add(point); // memory exception
                // prepare for next calculation
                nLastDiffX = nX;
                nLastDiffY = nY;
            }
            // return result
            var res = result.ToArray();
            return result.ToArray();
        }
        // Read one integer from compressed geometry string by using passed position
        // Returns extracted integer, and re-writes nStartPos for the next integer
        private int ExtractInt(string src, ref int nStartPos) // exception
        {
            bool bStop = false;
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            int nCurrentPos = nStartPos;
            while (!bStop)
            {
                char cCurrent = src[nCurrentPos];
                if (cCurrent == '+' || cCurrent == '-')
                {
                    if (nCurrentPos != nStartPos)
                    {
                        bStop = true;
                        continue;
                    }
                }
                result.Append(cCurrent); // exception
                nCurrentPos++;
                if (nCurrentPos == src.Length) // check overflow
                    bStop = true;
            }
            int nResult = int.MinValue;
            if (result.Length != 0)
            {
                nResult = FromStringRadix32(result.ToString());
                nStartPos = nCurrentPos;
            }
            return nResult;
        }
        // Sample input and output: +1lmo -> 55000
        private int FromStringRadix32(string s) // exception
        {
            int result = 0;
            for (int i = 1; i < s.Length; i++)
            {
                char cur = s[i];
                System.Diagnostics.Debug.Assert((cur >= '0' && cur <= '9') || (cur >= 'a' && cur <= 'v'));
                if (cur >= '0' && cur <= '9')
                    result = (result << 5) + System.Convert.ToInt32(cur) - System.Convert.ToInt32('0');
                else if (cur >= 'a' && cur <= 'v')
                    result = (result << 5) + System.Convert.ToInt32(cur) - System.Convert.ToInt32('a') + 10;
                else throw new System.ArgumentOutOfRangeException(); // exception
            }
            if (s[0] == '-')
                result = -result;
            else if (s[0] != '+')
                throw new System.ArgumentOutOfRangeException(); // exception
            return result;
        }
    }
}
