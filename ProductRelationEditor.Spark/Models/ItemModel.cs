namespace ProductRelationEditor.Spark.Models
{
    public class ItemModel
    {
        public string TEC_KIND { get; set; }
        public string CCATEGORY { get; set; }
        public string EQP_ID { get; set; }
        public string GNAME { get; set; }
        public string PRODUCT { get; set; }
        public string P_RECIPE { get; set; }
        public string TIMESERIES_SEQ_NO { get; set; }
        public string DCITEM_NM { get; set; }
        public string DCITEM_UNIT { get; set; }
        public string CTITLE { get; set; }

        public ItemModel(
            string tecKind,
            string ccategory,
            string eqpId,
            string gname,
            string product,
            string pRecipe,
            string timeseriesSeqNo,
            string dcitemNm,
            string dcitemUnit,
            string ctitle)
        {
            TEC_KIND = tecKind;
            CCATEGORY = ccategory;
            EQP_ID = eqpId;
            GNAME = gname;
            PRODUCT = product;
            P_RECIPE = pRecipe;
            TIMESERIES_SEQ_NO = timeseriesSeqNo;
            DCITEM_NM = dcitemNm;
            DCITEM_UNIT = dcitemUnit;
            CTITLE = ctitle;
        }
    }
}
