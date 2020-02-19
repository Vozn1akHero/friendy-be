namespace BE.ElasticSearch
{
    public class ElasticConnectionSettings
    {
        private string _defaultIndex;
        public string ClusterUrl { get; set; }

        public string DefaultIndex
        {
            get => _defaultIndex;
            set => _defaultIndex = value.ToLower();
        }
    }
}