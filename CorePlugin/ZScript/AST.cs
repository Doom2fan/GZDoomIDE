using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePlugin.ZScript {
    public struct ZSIntLiteral {
        public enum NumberType {
            DecLiteral,
            HexLiteral,
        }

        public NumberType LiteralType { get; set; }
        public string Value { get; set; }

        public ZSIntLiteral (NumberType type, string value) {
            LiteralType = type;
            Value = value;
        }
    }

    public struct ZSFloatLiteral {
        public string Value { get; set; }

        public ZSFloatLiteral (string value) {
            Value = value;
        }
    }

    public struct ZSIdentifier {
        public string Name { get; set; }

        public ZSIdentifier (string name) {
            Name = name;
        }
    }
}
