using Pixstock.Common.Model;

namespace pixstock.apl.app.Models
{
    public class Label : ILabel
    {
        public string Name { get; set; }
        public string MetaType { get; set; }
        public string Comment { get; set; }
        public long Id { get; set; }
    }
}