public class ItemModel
{
    // ...（プロパティはそのまま）

    public ItemModel(
        string TEC_KIND,
        string CCATEGORY,
        string EQP_ID,
        string GNAME,
        string PRODUCT,
        string P_RECIPE,
        string M_PROCESS_NAME,
        string M_WORK_NAME,
        string M_FLD_ID,
        string M_WORK_CODE,
        string TIMESERIES_SEQ_NO,
        string DCITEM_NM,
        string DCITEM_UNIT,
        string CTITLE,
        bool SENDFLG,
        string LARGE_GROUP,
        string SMALL_GROUP,
        string DISPLAY_NAME,
        string CHAMBER_NAME,
        bool POINTFLG
    )
    {
        this.TEC_KIND = TEC_KIND;
        this.CCATEGORY = CCATEGORY;
        this.EQP_ID = EQP_ID;
        this.GNAME = GNAME;
        this.PRODUCT = PRODUCT;
        this.P_RECIPE = P_RECIPE;
        this.M_PROCESS_NAME = M_PROCESS_NAME;
        this.M_WORK_NAME = M_WORK_NAME;
        this.M_FLD_ID = M_FLD_ID;
        this.M_WORK_CODE = M_WORK_CODE;
        this.TIMESERIES_SEQ_NO = TIMESERIES_SEQ_NO;
        this.DCITEM_NM = DCITEM_NM;
        this.DCITEM_UNIT = DCITEM_UNIT;
        this.CTITLE = CTITLE;
        this.SENDFLG = SENDFLG;
        this.LARGE_GROUP = LARGE_GROUP;
        this.SMALL_GROUP = SMALL_GROUP;
        this.DISPLAY_NAME = DISPLAY_NAME;
        this.CHAMBER_NAME = CHAMBER_NAME;
        this.POINTFLG = POINTFLG;
    }
}