namespace EditorService.Common.Dto
{
    public class ItemModel
    {
        public string TEC_KIND { get; set; }
        public string CCATEGORY { get; set; }
        public string EQP_ID { get; set; }
        public string GNAME { get; set; }
        public string PRODUCT { get; set; }
        public string P_RECIPE { get; set; }
        public string M_PROCESS_NAME { get; set; }
        public string M_WORK_NAME { get; set; }
        public string M_FLD_ID { get; set; }
        public string M_WORK_CODE { get; set; }
        public string TIMESERIES_SEQ_NO { get; set; }
        public string DCITEM_NM { get; set; }
        public string DCITEM_UNIT { get; set; }
        public string CTITLE { get; set; }
        public bool SENDFLG { get; set; }
        public string LARGE_GROUP { get; set; }
        public string SMALL_GROUP { get; set; }
        public string DISPLAY_NAME { get; set; }
        public string CHAMBER_NAME { get; set; }
        public bool POINTFLG { get; set; }

        public ItemModel(
                    string tecKind,
                    string ccategory,
                    string eqpId,
                    string gname,
                    string product,
                    string pRecipe,
                    string mprocessnema,
                    string mworkname,
                    string mfldid,
                    string mworkcode,
                    string timeseriesSeqNo,
                    string dcitemNm,
                    string dcitemUnit,
                    string ctitle,
                    bool sendflg,
                    string lagegroup,
                    string smallgroup,
                    string displayname,
                    string chambername,
                    bool pointflg
                    )
        {
            TEC_KIND = tecKind;
            CCATEGORY = ccategory;
            EQP_ID = eqpId;
            GNAME = gname;
            PRODUCT = product;
            P_RECIPE = pRecipe;
            M_PROCESS_NAME = mprocessnema;
            M_WORK_NAME = mworkname;
            M_FLD_ID = mfldid;
            M_WORK_CODE = mworkcode;
            TIMESERIES_SEQ_NO = timeseriesSeqNo;
            DCITEM_NM = dcitemNm;
            DCITEM_UNIT = dcitemUnit;
            CTITLE = ctitle;
            SENDFLG = sendflg;
            LARGE_GROUP = lagegroup;
            SMALL_GROUP = smallgroup;
            DISPLAY_NAME = displayname;
            CHAMBER_NAME = chambername;
            POINTFLG = pointflg;
        }
    }
}