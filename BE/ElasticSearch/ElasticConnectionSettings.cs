namespace BE.ElasticSearch
{
    public class ElasticConnectionSettings
    {
        public string ClusterUrl { get; set; }

        public string DefaultIndex
        {
            get
            {
                return this._defaultIndex;
            }
            set
            {
                this._defaultIndex = value.ToLower();
            }
        }

        private string _defaultIndex;
    }
}