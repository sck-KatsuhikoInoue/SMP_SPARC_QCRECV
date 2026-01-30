using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorService
{
    // 検索パラメータ
    public class SpcMasterParameter
    {
        public string TEC_KIND { get; set; }
        public string CCATEGORY { get; set; }
        public string EQP_ID { get; set; }
        public string GNAME { get; set; }
    }

    // 検索結果
    public class SpcMasterResult
    {

        public string TEC_KIND { get; set; }
        public string DC_TYPE { get; set; }
        public int GNO { get; set; }
        public int SUB_GNO { get; set; }
        public int CNO { get; set; }
        public string CCATEGORY { get; set; }
        public string GNAME { get; set; }
        public string PRODUCT { get; set; }
        public string EQP_ID { get; set; }
        public string P_RECIPE { get; set; }
        public string P_WORK_CODE { get; set; }
        public string M_PROCESS_NAME { get; set; }
        public string M_WORK_NAME { get; set; }
        public string M_FLD_ID { get; set; }
        public string M_WORK_CODE { get; set; }
        public string DCITEM_NM { get; set; }
        public string DCITEM_UNIT { get; set; }
        public int CTYPE { get; set; }
        public string CNAME { get; set; }
        public string CTITLE { get; set; }
        public string TIMESERIES_SEQ_NO { get; set; }
        public DateTime ST_TIMESTAMP_M { get; set; }
        public string LARGE_GROUP { get; set; }
        public string SMALL_GROUP { get; set; }
        public int SENDFLG { get; set; }
        public int POINTFLG { get; set; }
        public string UP_USER { get; set; }
        public DateTime ST_TIMESTAMP_G { get; set; }
        public string DISPLAY_NAME { get; set; }
        public string CHAMBER_NAME { get; set; }
        public DateTime ST_TIMESTAMP_I { get; set; }
    }
}
