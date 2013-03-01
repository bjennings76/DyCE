using System.IO;
using System.Text;

namespace DyCE
{
    public class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding _encoding;

        public StringWriterWithEncoding(StringBuilder builder, Encoding encoding)
            : base(builder) { _encoding = encoding; }

        public override Encoding Encoding { get { return _encoding; } }
    }
}